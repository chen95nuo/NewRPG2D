//-----------------------------------------------------------------
//  Copyright 2010 Brady Wright and Above and Beyond Software
//	All rights reserved
//-----------------------------------------------------------------


// #define WARN_ON_NO_MATERIAL
#define SPRITE_WANT_NORMALS		// Automatically generates normals for the sprite mesh
// #define UV2_SUPPORT


using UnityEngine;
using System.Collections;


// Wraps the actual sprite mesh
// so that the sprite itself doesn't
// have to worry about the underlying
// management of the mesh directly
// so that it can easily switch between
// managed or unmanaged without worry.
public class SpriteMesh : ISpriteMesh
{
	protected SpriteRoot m_sprite;

	protected MeshFilter meshFilter;
	protected MeshRenderer meshRenderer;
	protected Mesh m_mesh;						// Reference to our mesh
	protected Texture m_texture;
	protected Vector3[] m_vertices;
	protected Color[] m_colors;
	protected Vector2[] m_uvs;
#if UV2_SUPPORT	
	protected Vector2[] m_uvs2;
#endif
	protected int[] m_faces;
#if UV2_SUPPORT	
	protected bool m_useUV2 = false;
#endif

	protected int m_vertexCountX;
	protected int m_vertexCountY;

	public virtual SpriteRoot sprite
	{
		get { return m_sprite; }
		set 
		{
			m_sprite = value;

			if (m_sprite != null)
			{
				if (m_sprite.spriteMesh != this)
					m_sprite.spriteMesh = this;
			}
			else 
				return;

			meshFilter = (MeshFilter)m_sprite.gameObject.GetComponent(typeof(MeshFilter));
			if (meshFilter == null)
				meshFilter = (MeshFilter) m_sprite.gameObject.AddComponent(typeof(MeshFilter));

			meshRenderer = (MeshRenderer)m_sprite.gameObject.GetComponent(typeof(MeshRenderer));
			if (meshRenderer == null)
				meshRenderer = (MeshRenderer)m_sprite.gameObject.AddComponent(typeof(MeshRenderer));

			m_mesh = meshFilter.sharedMesh;

			if (meshRenderer.sharedMaterial != null)
				m_texture = meshRenderer.sharedMaterial.GetTexture("_MainTex");
#if WARN_ON_NO_MATERIAL
			else
				Debug.LogWarning("Sprite on GameObject \"" + m_sprite.name + "\" has not been assigned a material.");
#endif
		}
	}

	public virtual Texture texture
	{
		get { return m_texture; }
	}

	public virtual Material material
	{
		get { return meshRenderer.sharedMaterial; }
		set 
		{ 
			meshRenderer.sharedMaterial = value;
			m_texture = meshRenderer.sharedMaterial.mainTexture;
			if (m_sprite != null && m_texture != null)
				m_sprite.SetPixelToUV(m_texture);
		}
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
#if UV2_SUPPORT	
		get { return m_uvs2; }
#else
		get { return null; }
#endif
	}

	public virtual int vertexCountX
	{
		get { return m_vertexCountX; }
	}

	public virtual int vertexCountY
	{
		get { return m_vertexCountY; }
	}

	public virtual bool UseUV2
	{
#if UV2_SUPPORT
		get { return m_useUV2; }
		set { m_useUV2 = value; }
#else
		get { return false; }
		set { ; }
#endif
	}

	public virtual Mesh mesh
	{
		get 
		{
			if (m_mesh == null)
			{
				CreateMesh();
			}

			return m_mesh; 
		}
		set { m_mesh = value; }
	}

	public virtual void Init()
	{
		
		if (m_mesh == null)
		{
			CreateMesh();
		}

		InitializeBuffers();

		// Assign to mesh object:
		m_mesh.Clear();
		m_mesh.vertices = m_vertices;
		m_mesh.uv = m_uvs;
		m_mesh.colors = m_colors;
		m_mesh.triangles = m_faces;

		// Calculate UV dimensions:
		if (!m_sprite.uvsInitialized)
		{
			m_sprite.InitUVs();
			m_sprite.uvsInitialized = true;
		}

		// Build vertices:
		if (m_sprite.pixelPerfect)
		{
			if (m_texture == null)
			{
				if (meshRenderer.sharedMaterial != null)
					m_texture = meshRenderer.sharedMaterial.GetTexture("_MainTex");
			}

			if (m_texture != null)
			{
				m_sprite.SetPixelToUV(m_texture);
			}

			if (m_sprite.renderCamera == null)
				m_sprite.SetCamera(Camera.main);
			else
				m_sprite.SetCamera(m_sprite.renderCamera);
		}
		else if (!m_sprite.hideAtStart)
			m_sprite.SetSize(m_sprite.width, m_sprite.height);

		m_sprite.SetBleedCompensation(m_sprite.bleedCompensation);

#if SPRITE_WANT_NORMALS
		m_mesh.RecalculateNormals();
#endif

		// Set colors:
		m_sprite.SetColor(m_sprite.color);
	}

	protected void CreateMesh()
	{
		meshFilter.sharedMesh = new Mesh();
		m_mesh = meshFilter.sharedMesh;

		if (m_sprite.persistent)
			GameObject.DontDestroyOnLoad(m_mesh);
	}

	protected bool InitializeBuffers()
	{
		bool dirty = false;
		
		int gridCountX;
		int gridCountY;
		bool pierced;
		sprite.CalcGridCount(out gridCountX, out gridCountY, out pierced);

		// Get vertex count
		int _vertexCountX = sprite.xTextureTile ? gridCountX * 2 : gridCountX + 1;
		int _vertexCountY = sprite.yTextureTile ? gridCountY * 2 : gridCountY + 1;
		
		if (m_vertexCountX != _vertexCountX || m_vertexCountY != _vertexCountY)
		{
			dirty = true;
			m_vertexCountX = _vertexCountX;
			m_vertexCountY = _vertexCountY;
		}

		// Get vertex
		int vertexCount = m_vertexCountX * m_vertexCountY;
		
		// Initialize vertex buffer
		if (m_vertices == null || m_vertices.Length != vertexCount)
		{
			dirty = true;
			m_vertices = new Vector3[vertexCount];
			m_colors = new Color[vertexCount];
			m_uvs = new Vector2[vertexCount];
#if UV2_SUPPORT	
			m_uvs2 = new Vector2[vertexCount];
#endif
		}

		// Get face count
		int faceCount = gridCountX * gridCountY * 2;
		if (pierced)
			faceCount -= (gridCountX - 2) * (gridCountY - 2) * 2; // Remove pierced faces

		// Initialize index buffer		
		if (m_faces == null || m_faces.Length != faceCount * 3)
		{
			dirty = true;
			m_faces = new int[faceCount * 3];
		}
		
		if (dirty)
		{
			int xFactor = m_sprite.xTextureTile ? 2 : 1;
			int yFactor = m_sprite.yTextureTile ? 2 : 1;
			
			for (int y = 0, index = 0; y < gridCountY; ++y)
			{
				for (int x = 0; x < gridCountX; ++x)
				{
					// Skip pierced face
					if (pierced && y > 0 && y < gridCountY - 1 && x > 0 && x < gridCountX - 1)
						continue;

					int vertexIndex = y * m_vertexCountX * yFactor + x * xFactor;
					
					m_faces[index++] = vertexIndex;
					m_faces[index++] = vertexIndex + 1;
					m_faces[index++] = vertexIndex + m_vertexCountX;
					
					m_faces[index++] = vertexIndex + 1;
					m_faces[index++] = vertexIndex + m_vertexCountX + 1;
					m_faces[index++] = vertexIndex + m_vertexCountX;
				}
			}
		}
		
		return dirty;
	}

	public virtual bool UpdateBuffers()
	{
		if (InitializeBuffers())
		{
			mesh.Clear();
			m_mesh.vertices = m_vertices;
			m_mesh.uv = m_uvs;
			m_mesh.colors = m_colors;
			m_mesh.triangles = m_faces;

			return true;
		}

		// Sometime m_mesh.triangles become empty, but m_faces is not empty. Dont know the reason, fix it temporarily
		if (m_mesh != null && m_faces != null && (m_mesh.triangles == null || m_mesh.triangles.Length != m_faces.Length))
			m_mesh.triangles = m_faces;

		return false;
	}

	public virtual void UpdateVerts()
	{
		if (m_mesh == null)
			CreateMesh();
		
		m_mesh.vertices = m_vertices;
		m_mesh.RecalculateBounds();
#if SPRITE_WANT_NORMALS
		m_mesh.RecalculateNormals();
#endif
	}

	public virtual void UpdateUVs()
	{
		if (m_mesh != null)
		{
			m_mesh.uv = m_uvs;
#if UV2_SUPPORT	
			if (m_useUV2)
				m_mesh.uv2 = m_uvs2;
#endif
		}
	}

	public virtual void UpdateColors(Color color)
	{
		for (int idx = 0; idx < m_colors.Length; ++idx )
		{
			m_colors[idx] = color;
		}

		if (m_mesh != null)
		{
			m_mesh.colors = m_colors;
		}
	}

	public virtual void Hide(bool tf)
	{
		if (meshRenderer == null)
			return;

		meshRenderer.enabled = !tf;
	}

	public virtual bool IsHidden()
	{
		if (meshRenderer == null)
			return true;

		return !meshRenderer.enabled;
	}

	public void SetPersistent()
	{
		if(m_mesh != null)
			GameObject.DontDestroyOnLoad(m_mesh);
	}
}
