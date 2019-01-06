//-----------------------------------------------------------------
//  Copyright 2010 Brady Wright and Above and Beyond Software
//	All rights reserved
//-----------------------------------------------------------------


using UnityEngine;
using System.Collections;


// Wraps the management of a sprite's mesh.
// This way, the sprite can act as though it
// has its own mesh, while this class works
// with the SpriteManager to update the proper
// vertices, UVs, etc, in the managed mesh.
public class SpriteMesh_Managed : ISpriteMesh, IEZLinkedListItem<SpriteMesh_Managed>
{
	protected SpriteRoot m_sprite;

	protected bool hidden;

	public int index;
	public int drawLayer;
	public SpriteManager m_manager;

	// Used for stepping through lists:
	public SpriteMesh_Managed m_next;
	public SpriteMesh_Managed m_prev;

	// The sprite's vertices and such (local copy)
	protected Vector3[] m_vertices = new Vector3[4];
	protected Vector2[] m_uvs = new Vector2[4];
	protected Vector2[] m_uvs2 = new Vector2[4];
	protected bool m_useUV2 = false;
	protected Material m_material;
	protected Texture m_texture;

	// The entire manager's vertices:
	protected Vector3[] meshVerts;

	// The entire manager's UVs:
	protected Vector2[] meshUVs;
	protected Vector2[] meshUVs2;

	// The entire manager's colors:
	protected Color[] meshColors;

	public int mv1;							// Indices of the associated vertices in the actual mesh (this just provides a quicker way for the SpriteManager to get straight to the right vertices in the vertex array)
	public int mv2;
	public int mv3;
	public int mv4;

	public int uv1;							// Indices of the associated UVs in the mesh
	public int uv2;
	public int uv3;
	public int uv4;

	public int cv1;							// Indices of the associated color values in the mesh
	public int cv2;
	public int cv3;
	public int cv4;

	public void SetBuffers(Vector3[] verts, Vector2[] uvs, Vector2[] uvs2, Color[] cols)
	{
		meshVerts = verts;
		meshUVs = uvs;
		meshUVs2 = uvs2;
		meshColors = cols;
	}

	public void Clear()
	{
		hidden = false;
		//UpdateColors(Color.white);
	}

	public SpriteManager manager
	{
		get { return m_manager; }
		set
		{
			m_manager = value;

			m_material = m_manager.renderer.sharedMaterial;
				
			if(m_material != null)
				m_texture = m_material.GetTexture("_MainTex");
		}
	}

	public virtual SpriteRoot sprite
	{
		get { return m_sprite; }
		set 
		{ 
			m_sprite = value;
			if(m_sprite != null)
				UpdateColors(m_sprite.color);
		}
	}

	public virtual Texture texture
	{
		get { return m_texture; }
	}

	public virtual Material material
	{
		get { return m_material; }
	}

	public virtual Vector3[] vertices
	{
		get { return m_vertices; }
	}

	public virtual Vector2[] uvs
	{
		get { return m_uvs; }
	}

	public virtual Vector2[] uvs2
	{
		get { return m_uvs2; }
	}

	public virtual int vertexCountX
	{
		get { return 2; }
	}

	public virtual int vertexCountY
	{
		get { return 2; }
	}

	public virtual bool UseUV2
	{
		get { return m_useUV2; }
		set { m_useUV2 = value; }
	}

	public virtual void Init()
	{
		//throw new System.NotImplementedException();

		if (!m_sprite.Started)
			m_sprite.Start();

		// Calculate UV dimensions:
		if (!m_sprite.uvsInitialized)
		{
			m_sprite.InitUVs();
			m_sprite.uvsInitialized = true;
		}

		m_sprite.SetBleedCompensation(m_sprite.bleedCompensation);

		// Build vertices:
		if (m_sprite.pixelPerfect)
		{
			if (m_texture != null)
			{
				m_sprite.SetPixelToUV(m_texture);
			}

			if (m_sprite.renderCamera == null)
				m_sprite.SetCamera(Camera.main);
			else
				m_sprite.SetCamera(m_sprite.renderCamera);
		}
		else if(!m_sprite.hideAtStart)
			m_sprite.SetSize(m_sprite.width, m_sprite.height);

		// Set colors:
		m_sprite.SetColor(m_sprite.color);
	}

	public virtual bool UpdateBuffers()
	{
		return false;
	}
	
	public virtual void UpdateVerts()
	{
		// Only update our vertices if we
		// aren't hidden since we hide
		// managed sprites by keeping the
		// vertices at 0,0,0:
		if (hidden)
			return;
#if !UNITY_FLASH
		meshVerts[mv1] = m_vertices[0];
		meshVerts[mv2] = m_vertices[1];
		meshVerts[mv3] = m_vertices[2];
		meshVerts[mv4] = m_vertices[3];
#else
		meshVerts[mv1].x = m_vertices[0].x;	meshVerts[mv1].y = m_vertices[0].y;	meshVerts[mv1].z = m_vertices[0].z;
		meshVerts[mv2].x = m_vertices[1].x;	meshVerts[mv2].y = m_vertices[1].y;	meshVerts[mv2].z = m_vertices[1].z;
		meshVerts[mv3].x = m_vertices[2].x;	meshVerts[mv3].y = m_vertices[2].y;	meshVerts[mv3].z = m_vertices[2].z;
		meshVerts[mv4].x = m_vertices[3].x;	meshVerts[mv4].y = m_vertices[3].y;	meshVerts[mv4].z = m_vertices[3].z;
#endif

		m_manager.UpdatePositions();
	}

	public virtual void UpdateUVs()
	{
#if !UNITY_FLASH
		meshUVs[uv1] = uvs[0];
		meshUVs[uv2] = uvs[1];
		meshUVs[uv3] = uvs[2];
		meshUVs[uv4] = uvs[3];
#else
		meshUVs[uv1].x = uvs[0].x;	meshUVs[uv1].y = uvs[0].y;
		meshUVs[uv2].x = uvs[1].x;	meshUVs[uv2].y = uvs[1].y;
		meshUVs[uv3].x = uvs[2].x;	meshUVs[uv3].y = uvs[2].y;
		meshUVs[uv4].x = uvs[3].x;	meshUVs[uv4].y = uvs[3].y;
#endif

		if (m_useUV2)
		{
#if !UNITY_FLASH
			meshUVs2[uv1] = uvs2[0];
			meshUVs2[uv2] = uvs2[1];
			meshUVs2[uv3] = uvs2[2];
			meshUVs2[uv4] = uvs2[3];
#else
			meshUVs2[uv1].x = uvs2[0].x;	meshUVs2[uv1].y = uvs2[0].y;
			meshUVs2[uv2].x = uvs2[1].x;	meshUVs2[uv2].y = uvs2[1].y;
			meshUVs2[uv3].x = uvs2[2].x;	meshUVs2[uv3].y = uvs2[2].y;
			meshUVs2[uv4].x = uvs2[3].x;	meshUVs2[uv4].y = uvs2[3].y;
#endif
		}

		m_manager.UpdateUVs();
	}

	public virtual void UpdateColors(Color color)
	{
		meshColors[cv1] = color;
		meshColors[cv2] = color;
		meshColors[cv3] = color;
		meshColors[cv4] = color;

		m_manager.UpdateColors();
	}

	public virtual void Hide(bool tf)
	{
		if (tf)
		{
			m_vertices[0] = Vector3.zero;
			m_vertices[1] = Vector3.zero;
			m_vertices[2] = Vector3.zero;
			m_vertices[3] = Vector3.zero;
			UpdateVerts();
			hidden = tf; // Assign after so that UpdateVerts() will run.
		}
		else
		{
			hidden = tf;
			if (m_sprite.pixelPerfect)
				m_sprite.CalcSize();
			else
				m_sprite.SetSize(m_sprite.width, m_sprite.height);
		}
	}

	public virtual bool IsHidden()
	{
		return hidden;
	}

	public SpriteMesh_Managed prev
	{
		get { return m_prev; }
		set { m_prev = value; }
	}

	public SpriteMesh_Managed next
	{
		get { return m_next; }
		set { m_next = value; }
	}
}
