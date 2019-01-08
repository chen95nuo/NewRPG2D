using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class ZMNetData:ICloneable {
    public int pos;
    public byte isnot = 0;
    public short type;
    public byte[] data;
    public bool handled;
    private BinaryWriter outputCache = null;
    private MemoryStream segCache;
    public byte[] hand;

    public ZMNetData(int type) : this(type, false){

    }

    public ZMNetData(int type, byte inot) : this(type, inot, false) { 
    }

    public ZMNetData(int opcode, byte inot, bool needSerial)
    {
        type = (short)opcode;
        isnot = inot;
        try
        {
            segCache = new MemoryStream();
            outputCache = new BinaryWriter(segCache);

            outputCache.Write(type);
            outputCache.Write(isnot);
            /*if(needSerial) {
                serial = serialKey.nextKey();
                outputCache.writeInt(serial);        	
            }*/
        }
        catch (IOException ex)
        {
        }
    }
    public ZMNetData(int opcode, bool needSerial)
    {
        type = (short)opcode;
        isnot = (byte)2;
        try{
            segCache = new MemoryStream();
            outputCache = new BinaryWriter(segCache);

            outputCache.Write(type);
            outputCache.Write(isnot);
            /*if(needSerial) {
                serial = serialKey.nextKey();
                outputCache.writeInt(serial);        	
            }*/
        }catch(IOException ex){
        }
    }

    public ZMNetData(byte[] data)
    {
        this.data = new byte[data.Length];
        Array.Copy(data, 0, this.data, 0, data.Length);
        this.hand = new byte[2];
        hand = readHead(data);
        type = (short)getNumber(this.data, 2, 2);//头一共2个字节，从之后开始读取为OpCode和isnot,data
        isnot = this.data[4];
        pos = 5;//5字节之后的为实际内容
    //    Debug.Log("--------------------type:" + type);
    }

    public void streamToBytes(Stream stream)
    {
        data = new byte[stream.Length];
        // 设置当前流的位置为流的开始   
        stream.Seek(0, SeekOrigin.Begin);
        stream.Read(data, 0, data.Length);
    }  

    public void flush()
    {
        if (segCache == null)
        {
            return;
        }

        try
        {
            outputCache.Flush();
            streamToBytes(segCache);
            
        }
        catch (Exception e)
        {
        }
        finally
        {
            try
            {
                outputCache.Close();
            }
            catch (Exception e)
            {
            }
        }

        segCache = null;
        outputCache = null;

        setNumber(type, data, 0, 2);
    }

    public void reset()
    {
        pos = 2;
    }

    public static long getNumber(byte[] buf, int off, int len)
    {
        long longVal = 0;

        for (int i = 0; i < len; i++)
        {
            longVal <<= 8;
            longVal |= (buf[off + i]) & 0xff;
        }

        return longVal;
    }
    public static byte[] readHead(byte[] bbc)
    {
        byte[] bt = new byte[2];
        bt[0] = bbc[0];
        bt[1] = bbc[1];
        return bt;
    }

    public static byte[] readfront(byte[] bbc)
    {
        byte[] bt = new byte[8];
        bt[0] = bbc[0];
        bt[1] = bbc[1];
        bt[2] = bbc[2];
        bt[3] = bbc[3];
        bt[4] = bbc[4];
        bt[5] = bbc[5];
        bt[6] = bbc[6];
        bt[7] = bbc[7];
        return bt;
    }
    public bool readBoolean(){
        return (data[pos++] == (byte)1)? true: false;
    }

    public byte readByte(){
        return data[pos++];
    }
    
    public int readUnsignedByte(){
        return data[pos++] & 0xFF;
    }

    public short readShort(){
        pos += 2;

        return (short)getNumber(data, pos - 2, 2);
    }
    
    public int readUnsignedShort(){
        return readShort() & 0xFFFF;
    }

    public int readInt(){
        pos += 4;

        return (int)getNumber(data, pos - 4, 4);
    }
    
    public void setInt(int num){
        setNumber(num, data, pos, 4);
    }

    public long readLong(){
        pos += 8;
        return getNumber(data, pos - 8, 8);
    }
    /*
    public String[] readStrings()
    {
        int len = (int)getNumber(data, pos, 2);
        pos += 2;
        String[] result = new String[len];

        for (int i = 0; i < len; i++)
        {
            result[i] = readString();
        }

        return result;
    }
    */
    public String readString(){
        int len = (int)readShort();
        if ((len & 0x8000) == 0)
        {


            byte[] buf = readBytes();
          //  byte[] buf = new byte[len];
            /*
            for(int i = 0;i<len;i++)
            {
                buf[i] = readByte();
            }
             * */
            string str = System.Text.Encoding.UTF8.GetString(buf);
            return str;
        }
        
        /*
        NetworkStream dis = null;
        try {
            if ((data[pos] & 0x80) == 0) {
                dis = new DataInputStream(new ByteArrayInputStream(data, pos, data.Length - pos));
                pos += getNumber(data, pos, 2) + 2;
                return ByteStream.readUTF(dis);
            } else {
                int len = (int)(getNumber(data, pos, 2) & 0x7FFF);
                pos += 2;
                char[] charr = new char[len / 2];
                for (int i = 0; i < charr.length; i++) {
                    charr[i] = (char)(((data[pos] & 0xFF) << 8) | (data[pos + 1] & 0xFF));
                    pos += 2;
                }
                return new String(charr);
            }
        } catch (Exception e) {
            throw new Exception(e.ToString());
            return "";
        }finally{
            try{
                dis.close();
            }catch(Exception e){
            }
        }*/
        return "";
    }

    public bool[] readBooleans(){
        int len = (int)getNumber(data, pos, 2);
        pos += 2;
        bool[] result = new bool[len];

        for(int i = 0; i < len; i++){
            result[i] = readBoolean();
        }

        return result;
    }

    public byte[] readBytes(){
        int len = (int)getNumber(data, pos, 4);
        pos += 4;
        byte[] result = new byte[len];

        for(int i = 0; i < len; i++){
            result[i] = readByte();
        }

        return result;
    }

    public short[] readShorts(){
        int len = (int)getNumber(data, pos, 2);
        pos += 2;
        short[] result = new short[len];

        for(int i = 0; i < len; i++){
            result[i] = readShort();
        }

        return result;
    }

    public int[] readInts(){
        int len = (int)getNumber(data, pos, 2);
        pos += 2;
        int[] result = new int[len];

        for(int i = 0; i < len; i++){
            result[i] = readInt();
        }

        return result;
    }

    public long[] readLongs(){
        int len = (int)getNumber(data, pos, 2);
        pos += 2;
        long[] result = new long[len];

        for(int i = 0; i < len; i++){
            result[i] = readLong();
        }

        return result;
    }

    public double readDouble()
    {
        byte[] buf = new byte[8];
        for (int i = 0; i < 8; i++)
        {
            buf[i] = readByte();
        }
        double ret = System.BitConverter.ToDouble(buf,0);
    
        return ret;
    }


    public void writeBoolean(bool b) {
        if (b == true)
        {
            outputCache.Write((byte)1);
        }
        else if (b == false)
        {
            outputCache.Write((byte)0);
        }
        //outputCache.Write(b);   
    }

    public void writeByte(byte b) {
        outputCache.Write(b);
    }

    public void writeShort(short s) {
        outputCache.Write(s);
    }

    public void writeInt(int n) {
        
        outputCache.Write(n);
    }

    public void writeLong(long l) {
        outputCache.Write(l);
    }

    public void CZputString(String s)
    {
        outputCache.Write(s);
    }
    public string CZgetString()
    {
        byte[] bytes = new byte[data.Length-6];
        Array.Copy(data, 6, bytes, 0, bytes.Length);
        string str = System.Text.Encoding.UTF8.GetString(bytes);
        return str;
    }

    public void writeString(String s) {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
        outputCache.Write((short)bytes.Length);
        writeBytes(bytes);

    //    outputCache.Write((s == null)? "": s);
    }

    public void writeDouble(double d)
    {
        outputCache.Write(d);
    }

    public void writeUnsignedShot(UInt16 u) {
        outputCache.Write(u);
    }

    public void writeBooleans(bool[] b) {
        outputCache.Write(b.Length);

        for(int i = 0; i < b.Length; i++){
            outputCache.Write(b[i]);
        }
    }

    public void writeBytes(byte[] b) {
        outputCache.Write(b.Length);

        for(int i = 0; i < b.Length; i++){
            outputCache.Write(b[i]);
        }
    }

    public void writeShorts(short[] s) {
        outputCache.Write(s.Length);

        for(int i = 0; i < s.Length; i++){
            outputCache.Write(s[i]);
        }
    }

    public void writeInts(int[] n){
        outputCache.Write(n.Length);

        for(int i = 0; i < n.Length; i++){
            outputCache.Write(n[i]);
        }
    }
    /*
    public void writeObject(Object ob) {
        outputCache.Write(ob);
    }
    */
    public void writeLongs(long[] l) {
        outputCache.Write(l.Length);

        for(int i = 0; i < l.Length; i++){
            outputCache.Write(l[i]);
        }
    }

    public void writeStrings(String[] s) {
        outputCache.Write(s.Length);

        for(int i = 0; i < s.Length; i++){
            outputCache.Write(s[i]);
        }
    }

    public float getFloat()
    {
        byte[] buf = new byte[4];
        for (int i = 0; i < 4; i++)
        {
            buf[i] = readByte();
        }

        float ret = System.BitConverter.ToSingle(buf, 0);

        return ret;
    }

    public void putFloat(float f)
    {
        outputCache.Write(f);
    }

	public void putStrings(String[] s)
	{
		writeInt(s.Length);//写入数组长度
		for(int i=0;i<s.Length;i++)
		{
			writeString(s[i]);
		}
	}
	
	public String[] getStrings()
	{
		
		int length=readInt();//读取数组长度
		String[] getStrings=new String[length];
		for(int i=0;i<length;i++)
		{
			getStrings[i]=readString();
		}
		return getStrings;
	}

    public static void setNumber(int num, byte[] buf, int off, int len){
        for(int i = len - 1; i >= 0; i--){
            buf[off + i] = (byte)(num & 0xff);
            num >>= 8;
        }
    }

	#region ICloneable implementation

	public object Clone ()
	{
		return this.MemberwiseClone ();
	}

	#endregion
}
