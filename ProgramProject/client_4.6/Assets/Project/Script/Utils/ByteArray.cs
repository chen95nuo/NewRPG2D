using System;

public class ByteArray
{
	public const byte DEFAULT_SIZE = 16;
    public const byte BOOLEAN_SIZE = 1;
    public const byte BYTE_SIZE = 1;
    public const byte CHAR_SIZE = 2;
    public const byte SHORT_SIZE = 2;
    public const byte INT_SIZE = 4;
    public const byte LONG_SIZE = 8;
    private int currentPos = 0;
    private byte[] data;
	
    public ByteArray() {
    }

    public ByteArray(int size) {
        data = new byte[size];
        currentPos = 0;
    }

    public ByteArray(byte[] src) {
        data = src;
        currentPos = 0;
    }

    public void writeBoolean(bool val) {
        ensureCapacity(BOOLEAN_SIZE);
        data[currentPos++] = (byte) (val ? 1 : 0);
    }

    public void writeByte(byte val) {
        ensureCapacity(BYTE_SIZE);
        data[currentPos++] = val;
    }

    public void writeByte(int val) {
        writeByte((byte) val);
    }

    public void writeChar(char c) {
        ensureCapacity(CHAR_SIZE);
        data[currentPos + 1] = (byte) (c >> 0);
        data[currentPos + 0] = (byte) (c >> 8);
        currentPos += 2;
    }

    public void writeShort(short val) {
        ensureCapacity(SHORT_SIZE);
        data[currentPos + 1] = (byte) (val >> 0);
        data[currentPos + 0] = (byte) (val >> 8);
        currentPos += 2;
    }

    public void writeShort(int val) {
        writeShort((short) val);
    }

    public void writeInt(int val) {
        ensureCapacity(INT_SIZE);
        data[currentPos + 3] = (byte) (val >> 0);
        data[currentPos + 2] = (byte) (val >> 8);
        data[currentPos + 1] = (byte) (val >> 16);
        data[currentPos + 0] = (byte) (val >> 24);
        currentPos += INT_SIZE;
    }

    public void writeLong(long val) {
        ensureCapacity(LONG_SIZE);
        data[currentPos + 7] = (byte) (val >> 0);
        data[currentPos + 6] = (byte) (val >> 8);
        data[currentPos + 5] = (byte) (val >> 16);
        data[currentPos + 4] = (byte) (val >> 24);
        data[currentPos + 3] = (byte) (val >> 32);
        data[currentPos + 2] = (byte) (val >> 40);
        data[currentPos + 1] = (byte) (val >> 48);
        data[currentPos + 0] = (byte) (val >> 56);
        currentPos += LONG_SIZE;
    }

    public void writeByteArray(byte[] src) {
        if (src == null) {
            return;
        }
        ensureCapacity(src.Length);
		
		System.Array.Copy(src,0,data,currentPos,src.Length);
        currentPos += src.Length;
    }

    public void writeUTF(string str) {
    	if(str == null){
    		writeUTF("");
    		return;
    	}
        writeByteArray(getByteArrFromUTF(str));
    }

    public bool readBoolean() {
//        if(checkCapacity(BOOLEAN_SIZE)){
//            return false;
//        }
        return data[currentPos++] != 0;
    }

    public byte readByte() {
//        if(checkCapacity(BYTE_SIZE)){
//            return -1;
//        }
        return data[currentPos++];
    }

    public char readChar() {
//        if(checkCapacity(CHAR_SIZE)){
//            return '1';
//        }
        char c = (char) (((data[currentPos + 1] & 0xFF) << 0) |
                         ((data[currentPos + 0] & 0xFF) << 8));
        currentPos += CHAR_SIZE;
        return c;
    }

    public short readShort() {
        short s = (short) (((data[currentPos + 1] & 0xFF) << 0) |
                           ((data[currentPos + 0] & 0xFF) << 8));
        currentPos += SHORT_SIZE;
        return s;
    }

    public int readInt() {
        int i = ((data[currentPos + 3] & 0xFF) << 0) |
                ((data[currentPos + 2] & 0xFF) << 8)  |
                ((data[currentPos + 1] & 0xFF) << 16) |
                ((data[currentPos + 0] & 0xFF) << 24);
        currentPos += INT_SIZE;
        return i;
    }

    public long readLong() {
        long l = ((data[currentPos + 7] & 0xFFL) << 0) |
                 ((data[currentPos + 6] & 0xFFL) << 8) |
                 ((data[currentPos + 5] & 0xFFL) << 16) |
                 ((data[currentPos + 4] & 0xFFL) << 24) |
                 ((data[currentPos + 3] & 0xFFL) << 32) |
                 ((data[currentPos + 2] & 0xFFL) << 40) |
                 ((data[currentPos + 1] & 0xFFL) << 48) |
                 ((data[currentPos + 0] & 0xFFL) << 56);
        currentPos += LONG_SIZE;
        return l;
    }

    public byte[] readByteArray()
    {
    	byte[] temp = new byte[currentPos];
		System.Array.Copy(data,0,temp,0,currentPos);
        return temp;
    }
    
    public byte[] readByteArray(int length) {
        if (length == -1 || currentPos + length > data.Length) {
            length = data.Length - currentPos;
        }
        byte[] temp = new byte[length];
		System.Array.Copy(data, currentPos, temp, 0, length);
        currentPos += length;
        return temp;
    }

    public byte[] readByteArray(int off, int length) {
        if (length == -1 || off + length > data.Length) {
            length = data.Length - off;
        }
        byte[] temp = new byte[length];
		System.Array.Copy(data, off, temp, 0, length);
        return temp;
    }

    public string readUTF() {
        int utflen = readUnsignedShort();
        if (utflen == -1) {
            return null;
        }
        byte[] bytearr = null;
        char[] chararr = null;

        bytearr = readByteArray(utflen);
        if(utflen > bytearr.Length){
        	return null;
        }
        chararr = new char[utflen];

        int c, char2, char3;
        int count = 0;
        int chararr_count = 0;

        while (count < utflen) {
            c = (int) bytearr[count] & 0xff;
            if (c > 127) {
                break;
            }
            count++;
            chararr[chararr_count++] = (char) c;
        }

        while (count < utflen) {
            c = (int) bytearr[count] & 0xff;
            switch (c >> 4) {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7: /* 0xxxxxxx*/
                count++;
                chararr[chararr_count++] = (char) c;
                break;
            case 12:
            case 13: /* 110x xxxx   10xx xxxx*/
                count += 2;
                char2 = (int) bytearr[count - 1];
                chararr[chararr_count++] = (char) (((c & 0x1F) << 6) |
                        (char2 & 0x3F));
                break;
            case 14: /* 1110 xxxx  10xx xxxx  10xx xxxx */
                count += 3;
                char2 = (int) bytearr[count - 2];
                char3 = (int) bytearr[count - 1];
                chararr[chararr_count++] = (char) (((c & 0x0F) << 12) |
                        ((char2 & 0x3F) << 6) |
                        ((char3 & 0x3F) << 0));
                break;
            default:
                break;
            }
        }
        return new String(chararr, 0, chararr_count);
    }

    /**
     * 检测data数组是否足够长
     * @param length int
     */
    private void ensureCapacity(int length) {
        if (currentPos + length >= data.Length) {
            byte[] tmp = new byte[data.Length + 2 * length];
			System.Array.Copy(data, 0, tmp, 0, data.Length);
            data = tmp;
        }
    }
    
    public int size(){
    	return data.Length;
    }
    
    public int length(){
    	return data.Length;
    }

    public static byte[] getByteArrFromUTF(String str) {
        int strlen = str.Length;
        int utflen = 0;
        int c, count = 0;

        /* use charAt instead of copying String to char array */
        for (int i = 0; i < strlen; i++) {
            c = str[i];
            if ((c >= 0x0001) && (c <= 0x007F)) {
                utflen++;
            } else if (c > 0x07FF) {
                utflen += 3;
            } else {
                utflen += 2;
            }
        }

        byte[] bytearr = new byte[utflen + 2];

        bytearr[count++] = (byte) ((utflen >> 8) & 0xFF);
        bytearr[count++] = (byte) ((utflen >> 0) & 0xFF);

//        for (int i = 0; i < strlen; i++) {
//            c = str[i];
//            if (!((c >= 0x0001) && (c <= 0x007F))) {
//                break;
//            }
//            bytearr[count++] = (byte) c;
//        }

        for (int i = 0; i < strlen; i++) {
            c = str[i];
            if ((c >= 0x0001) && (c <= 0x007F)) {
                bytearr[count++] = (byte) c;

            } else if (c > 0x07FF) {
                bytearr[count++] = (byte) (0xE0 | ((c >> 12) & 0x0F));
                bytearr[count++] = (byte) (0x80 | ((c >> 6) & 0x3F));
                bytearr[count++] = (byte) (0x80 | ((c >> 0) & 0x3F));
            } else {
                bytearr[count++] = (byte) (0xC0 | ((c >> 6) & 0x1F));
                bytearr[count++] = (byte) (0x80 | ((c >> 0) & 0x3F));
            }
        }
        return bytearr;
    }

    private int readUnsignedByte() {
        return data[currentPos++] & 0x00FF;
    }

    private int readUnsignedShort() {
        int ch1 = readUnsignedByte();
        int ch2 = readUnsignedByte();
        if ((ch1 | ch2) < 0) {
            return -1;
        }
        return (ch1 << 8) + (ch2 << 0);
    }
    
    public void append(byte[] data){
    	writeByteArray(data);
    }

    public byte[] toArray() {
        if (currentPos < data.Length) {
            return readByteArray(0, currentPos);
        }
        return data;
    }

    public void resetPosition(){
        currentPos = 0;
    }

    public void close() {
        data = null;
    }

    public static int[] bytesToInts(byte[] bytes){
        if(bytes == null || bytes.Length < 4){
            return null;
        }
        int[] ints = new int[bytes.Length >> 2];
        ByteArray ba = new ByteArray(bytes);
        for(int i=0,kk=ints.Length; i<kk; i++){
            ints[i] = ba.readInt();
        }
        return ints;
    }
    
    public static byte[] intsToBytes(int[] ints){
        if(ints == null || ints.Length <= 0){
            return null;
        }
        byte[] bytes = new byte[ints.Length << 2];
        ByteArray ba = new ByteArray(bytes);
        for(int i=0,kk=ints.Length; i<kk; i++){
            ba.writeInt(ints[i]);
        }
        return ba.toArray();
    }
    
    public byte[] subArray(int start, int end) {
        if (start > end) {
            return null;
        }

        byte[] tmp = new byte[end - start];
		System.Array.Copy(data, start, tmp, 0, tmp.Length);
        return tmp;
    }
    
    public String toString(){
    	return "ByteArray" + currentPos;
    }
    
}