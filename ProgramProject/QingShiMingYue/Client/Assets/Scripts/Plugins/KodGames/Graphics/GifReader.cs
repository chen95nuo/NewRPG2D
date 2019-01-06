using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace KodGames
{
	//public class GifReader
	//{
	//    const int VERSION_STRING_LENGTH = 3;
	//    const int GIF_SIGNATURE_LENGTH = 3;
	//    const int MAX_COLOURS_IN_COLOUR_TABLE = 256;
	//    const int CHAR_BIT = 8;
	//    const int MAX_FRAMES_ALLOWED = 128;
	//    const int HEADER_SIZE = VERSION_STRING_LENGTH + GIF_SIGNATURE_LENGTH;


	//    private byte[] gifSignatureGIF = null;
	//    private byte[] versionString89a = null;

	//    private const byte gifTrailer = 0x3B;
	//    private const byte imageSeperator = 0x2C;
	//    private const byte graphicControlExtLabel = 0xF9;
	//    private const byte commentExtLabel = 0xFE;
	//    private const byte plainTextExtLabel = 0x01;
	//    private const byte applicationExtLabel = 0xFF;

	//    private List<byte[][]> localColourTableArray = null;
	//    private List<byte> LZWminCodeSizeArray = null;
	//    private List<byte[]> compressedData = null;
	//    private List<uint> compressedDataLength = null;

	//    private List<byte[]> decompressedData = null;
	//    //private List<ulong> decompressedDataLength = null;

	//    public GifReader()
	//    {
	//        gifSignatureGIF = new byte[3];
	//        gifSignatureGIF[0] = (byte)'G';
	//        gifSignatureGIF[1] = (byte)'I';
	//        gifSignatureGIF[2] = (byte)'F';

	//        versionString89a = new byte[3];
	//        versionString89a[0] = (byte)'8';
	//        versionString89a[1] = (byte)'9';
	//        versionString89a[2] = (byte)'a';

	//        gifHeader = new HEADER();
	//        screenDescriptor = new LOGICAL_SCREEN_DESCRIPTOR();
	//        globalImageData = new GLOBAL_IMAGE_DATA();
	//        globalColourTable = new byte[MAX_COLOURS_IN_COLOUR_TABLE][];
	//        for (int i = 0; i < MAX_COLOURS_IN_COLOUR_TABLE; i++)
	//        {
	//            globalColourTable[i] = new byte[3];
	//        }

	//        imageDescriptorForCopy = new IMAGE_DESCRIPTOR();
	//        imageDescriptorArray = new List<IMAGE_DESCRIPTOR>();
	//        imageDataArray = new List<IMAGE_DATA>();
	//        localColourTableArray = new List<byte[][]>();
	//        LZWminCodeSizeArray = new List<byte>();
	//        compressedData = new List<byte[]>();
	//        compressedDataLength = new List<uint>();
	//        graphicControl = new GRAPHIC_CONTROL_BLOCK();
	//        graphicDataArray = new List<GRAPHIC_CONTROL_DATA>();
	//        graphicControlArray = new List<GRAPHIC_CONTROL_BLOCK>();

	//        stringTable = new List<byte[]>();
	//        //sizeInBytesOfStringInTable = new List<int>();

	//        decompressedData = new List<byte[]>();
	//        //decompressedDataLength = new List<ulong>();
	//    }

	//    private struct HEADER
	//    {
	//        public byte[] gifSignature;
	//        public byte[] versionString;

	//        public void Read(byte[] buffer, ref int src_offset)
	//        {
	//            gifSignature = new byte[GIF_SIGNATURE_LENGTH];
	//            versionString = new byte[VERSION_STRING_LENGTH];
	//            int dest_offset = 0;
	//            GifReader.ReadBytes(buffer, ref src_offset, ref gifSignature, ref dest_offset, GIF_SIGNATURE_LENGTH);
	//            dest_offset = 0;
	//            GifReader.ReadBytes(buffer, ref src_offset, ref versionString, ref dest_offset, VERSION_STRING_LENGTH);
	//        }
	//    };
	//    private HEADER gifHeader;

	//    private struct LOGICAL_SCREEN_DESCRIPTOR
	//    {
	//        public byte widthl;
	//        public byte widthh;
	//        public byte heightl;
	//        public byte heighth;
	//        public byte packedFields;
	//        public byte backgroundColourIndex;
	//        public byte pixelAspectRatio;

	//        public void Read(byte[] buffer, ref int offset)
	//        {
	//            widthl = ImageFileLoader.ReadByte(buffer, ref offset);
	//            widthh = ImageFileLoader.ReadByte(buffer, ref offset);
	//            heightl = ImageFileLoader.ReadByte(buffer, ref offset);
	//            heighth = ImageFileLoader.ReadByte(buffer, ref offset);
	//            packedFields = ImageFileLoader.ReadByte(buffer, ref offset);
	//            backgroundColourIndex = ImageFileLoader.ReadByte(buffer, ref offset);
	//            pixelAspectRatio = ImageFileLoader.ReadByte(buffer, ref offset);
	//        }
	//    };

	//    private LOGICAL_SCREEN_DESCRIPTOR screenDescriptor;
	//    const int LOGICAL_SCREEN_DESCRIPTOR_SIZE = 7;

	//    private struct GLOBAL_IMAGE_DATA
	//    {
	//        public ushort width;
	//        public ushort height;
	//        public ushort globalColourTableFollows;
	//        public ushort numberOfBitsPerColourInOrigional;
	//        public ushort isGlobalColourTableSorted;
	//        public ushort entriesInGlobalColourTable;
	//        public ushort readBufferLengthGlobalColourTable;
	//        public ushort indexOfBackgroundColour;
	//    };

	//    private GLOBAL_IMAGE_DATA globalImageData;

	//    private struct IMAGE_DESCRIPTOR
	//    {
	//        public byte imageSeperator;//Must contain 0x2c
	//        public byte xposl;
	//        public byte xposh;
	//        public byte yposl;
	//        public byte yposh;
	//        public byte widthl;
	//        public byte widthh;
	//        public byte heightl;
	//        public byte heighth;
	//        public byte packedFields;

	//        public void Read(byte[] buffer, ref int offset)
	//        {
	//            imageSeperator = ImageFileLoader.ReadByte(buffer, ref offset);
	//            xposl = ImageFileLoader.ReadByte(buffer, ref offset);
	//            xposh = ImageFileLoader.ReadByte(buffer, ref offset);
	//            yposl = ImageFileLoader.ReadByte(buffer, ref offset);
	//            yposh = ImageFileLoader.ReadByte(buffer, ref offset);
	//            widthl = ImageFileLoader.ReadByte(buffer, ref offset);
	//            widthh = ImageFileLoader.ReadByte(buffer, ref offset);
	//            heightl = ImageFileLoader.ReadByte(buffer, ref offset);
	//            heighth = ImageFileLoader.ReadByte(buffer, ref offset);
	//            packedFields = ImageFileLoader.ReadByte(buffer, ref offset);
	//        }
	//    };

	//    private IMAGE_DESCRIPTOR imageDescriptorForCopy;//Used for reading purposes.
	//    private List<IMAGE_DESCRIPTOR> imageDescriptorArray = null;
	//    private struct IMAGE_DATA
	//    {
	//        public ushort x;
	//        public ushort y;
	//        public ushort w;
	//        public ushort h;
	//        public ushort colourTableFollows;
	//        public ushort imageIsInterlaced;//This class will not process interlaced images.
	//        public ushort colourTableIsSorted;
	//        public ushort entriesInLocalColourTable;
	//        public ushort readBufferLengthLocalColourTable;
	//    };

	//    private List<IMAGE_DATA> imageDataArray;

	//    private struct GRAPHIC_CONTROL_BLOCK
	//    {
	//        public byte extensionIntro;//must be 0x21
	//        public byte craphicsControlLabel;//must be 0xF9
	//        public byte blockSize;//must be 4
	//        public byte packedFields;
	//        public byte delayTimel;
	//        public byte delayTimeh;
	//        public byte transparentIndex;
	//        public byte terminator;

	//        public void Read(byte[] buffer, ref int offset)
	//        {
	//            extensionIntro = ImageFileLoader.ReadByte(buffer, ref offset);
	//            craphicsControlLabel = ImageFileLoader.ReadByte(buffer, ref offset);
	//            blockSize = ImageFileLoader.ReadByte(buffer, ref offset);
	//            packedFields = ImageFileLoader.ReadByte(buffer, ref offset);
	//            delayTimel = ImageFileLoader.ReadByte(buffer, ref offset);
	//            delayTimeh = ImageFileLoader.ReadByte(buffer, ref offset);
	//            transparentIndex = ImageFileLoader.ReadByte(buffer, ref offset);
	//            terminator = ImageFileLoader.ReadByte(buffer, ref offset);
	//        }
	//    };
	//    //For copy purposes only
	//    GRAPHIC_CONTROL_BLOCK graphicControl;
	//    private List<GRAPHIC_CONTROL_BLOCK> graphicControlArray;

	//    struct GRAPHIC_CONTROL_DATA
	//    {
	//        public ushort disposalMethod;//0,1,2,3,4
	//        public ushort userInput;//will be ignored by this class.
	//        public ushort isTransparency;
	//        public ushort delayForImage;//1/100 of a second.
	//        public ushort indexOfTransparencyColourInTable;


	//    };
	//    //Will be added to for every image even if only null pointer
	//    private List<GRAPHIC_CONTROL_DATA> graphicDataArray;

	//    private struct COMMENT_EXTENSION
	//    {
	//        public byte extensionIntro;//must be 0x21
	//        public byte commentLabel;//must be 0xFE
	//        //unsigned char dataSize;

	//        public void Read(byte[] buffer, ref int offset)
	//        {
	//            extensionIntro = ImageFileLoader.ReadByte(buffer, ref offset);
	//            commentLabel = ImageFileLoader.ReadByte(buffer, ref offset);
	//        }
	//    };

	//    private struct PLAIN_TEXT_EXTENSION
	//    {
	//        public byte introducer;//must be 0x21
	//        public byte plainTextLaible;//must be 0x01
	//        public byte blockSize;//must be 12
	//        public ushort x;
	//        public ushort y;
	//        public ushort w;
	//        public ushort h;
	//        public byte celw;
	//        public byte celh;
	//        public byte tfc;
	//        public byte tbc;

	//        public void Read(byte[] buffer, ref int offset)
	//        {
	//            introducer = ImageFileLoader.ReadByte(buffer, ref offset);
	//            plainTextLaible = ImageFileLoader.ReadByte(buffer, ref offset);
	//            blockSize = ImageFileLoader.ReadByte(buffer, ref offset);
	//            x = (ushort)ImageFileLoader.ReadShort(buffer, ref offset);
	//            y = (ushort)ImageFileLoader.ReadShort(buffer, ref offset);
	//            w = (ushort)ImageFileLoader.ReadShort(buffer, ref offset);
	//            h = (ushort)ImageFileLoader.ReadShort(buffer, ref offset);
	//            celw = ImageFileLoader.ReadByte(buffer, ref offset);
	//            celh = ImageFileLoader.ReadByte(buffer, ref offset);
	//            tfc = ImageFileLoader.ReadByte(buffer, ref offset);
	//            tbc = ImageFileLoader.ReadByte(buffer, ref offset);
	//        }
	//    };

	//    struct APPLICATION_EXTENSION
	//    {
	//        public byte introducer;//must be 0x21
	//        public byte appExteLabel;//must be 0xFF
	//        public byte blockSize;//must be 11
	//        public byte[] appIdentifye;//your name?
	//        public byte[] appCode;

	//        public void Read(byte[] buffer, ref int src_offset)
	//        {
	//            appIdentifye = new byte[8];
	//            appCode = new byte[3];

	//            introducer = ImageFileLoader.ReadByte(buffer, ref src_offset);
	//            appExteLabel = ImageFileLoader.ReadByte(buffer, ref src_offset);
	//            blockSize = ImageFileLoader.ReadByte(buffer, ref src_offset);
	//            int dest_offset = 0;
	//            GifReader.ReadBytes(buffer, ref src_offset, ref appIdentifye, ref dest_offset, 8);
	//            dest_offset = 0;
	//            GifReader.ReadBytes(buffer, ref src_offset, ref appCode, ref dest_offset, 3);
	//        }
	//    };

	//    struct mRGB
	//    {
	//        byte r;
	//        byte g;
	//        byte b;
	//    };

	//    byte[] readBuffer;
	//    int readBufferLength;
	//    int bufferIndex;

	//    private byte[][] globalColourTable;

	//    public static void ReadBytes(byte[] source, ref int src_offset, ref byte[] dest, ref int des_offset, int count)
	//    {
	//        //Debug.Log("ReadBytes count " + count);
	//        //Debug.Log("ReadBytes offset pre " + offset);
	//        for (int i = 0; i < count; i++)
	//        {
	//            //Debug.Log("ReadBytes offset " + offset);
	//            dest[des_offset++] = source[src_offset++];
	//        }
	//        //Debug.Log("ReadBytes offset after " + offset);
	//    }

	//    private bool isBytesEqual(byte[] b1, byte[] b2, int count)
	//    {
	//        for (int i = 0; i < count; i++)
	//        {
	//            if (b1[i] != b2[i])
	//            {
	//                return false;
	//            }
	//        }

	//        return true;
	//    }

	//    public bool IsGIFFile(byte[] buffer)
	//    {
	//        int offset = 0;
	//        //gifHeader = new HEADER(GIF_SIGNATURE_LENGTH, VERSION_STRING_LENGTH);
	//        gifHeader.Read(buffer, ref offset);

	//        bool isSame = false;
	//        isSame = isBytesEqual(gifHeader.gifSignature, gifSignatureGIF, GIF_SIGNATURE_LENGTH);
	//        if (isSame)
	//        {
	//            isSame = isBytesEqual(gifHeader.versionString, versionString89a, VERSION_STRING_LENGTH);
	//        }

	//        return isSame;

	//    }

	//    private bool Load(byte[] bufferIn)
	//    {
	//        bool result = false;
	//        bufferIndex = 0;
	//        readBuffer = bufferIn;
	//        readBufferLength = bufferIn.Length;

	//        gifHeader.Read(readBuffer, ref bufferIndex);

	//        bool isSame = false;
	//        isSame = isBytesEqual(gifHeader.gifSignature, gifSignatureGIF, GIF_SIGNATURE_LENGTH);
	//        if (isSame)
	//        {
	//            isSame = isBytesEqual(gifHeader.versionString, versionString89a, VERSION_STRING_LENGTH);
	//        }
	//        if (!isSame)
	//        {
	//            return false;
	//        }

	//        screenDescriptor.Read(readBuffer, ref bufferIndex);
	//        globalImageData.width = (ushort)((screenDescriptor.widthh << 8) | (screenDescriptor.widthl << 0));
	//        globalImageData.height = (ushort)((screenDescriptor.heighth << 8) | (screenDescriptor.heightl << 0));
	//        globalImageData.indexOfBackgroundColour = screenDescriptor.backgroundColourIndex;

	//        byte colourTableMask = 0x80;//10000000 is 128 msb 1
	//        byte colourResMask = 0x70;//01110000 is 112
	//        byte sortMask = 0x08;//00001000 is 8
	//        byte sizeOfColourMask = 0x07;//00000111 is 7

	//        if ((screenDescriptor.packedFields & colourTableMask) == colourTableMask)
	//        {
	//            globalImageData.globalColourTableFollows = 1;
	//        }
	//        else
	//        {
	//            globalImageData.globalColourTableFollows = 0;
	//        }

	//        byte cres = (byte)((screenDescriptor.packedFields & colourResMask) >> 4);
	//        globalImageData.numberOfBitsPerColourInOrigional = 0;
	//        globalImageData.numberOfBitsPerColourInOrigional += cres;
	//        globalImageData.numberOfBitsPerColourInOrigional += 1;

	//        if ((screenDescriptor.packedFields & sortMask) == sortMask)
	//        {
	//            globalImageData.isGlobalColourTableSorted = 1;
	//        }
	//        else
	//        {
	//            globalImageData.isGlobalColourTableSorted = 0;
	//        }

	//        byte ctsize = (byte)((screenDescriptor.packedFields & sizeOfColourMask) >> 0);
	//        ctsize += 1;
	//        double x = 2;
	//        double y = ctsize;
	//        double res = System.Math.Pow(x, y);

	//        globalImageData.entriesInGlobalColourTable = (ushort)res;
	//        //Debug.Log("globalImageData.entriesInGlobalColourTable " + globalImageData.entriesInGlobalColourTable);
	//        if (globalImageData.globalColourTableFollows != 0)
	//        {
	//            for (int n = 0; n < globalImageData.entriesInGlobalColourTable; n++)
	//            {
	//                globalColourTable[n][0] = ImageFileLoader.ReadByte(readBuffer, ref bufferIndex);
	//                globalColourTable[n][1] = ImageFileLoader.ReadByte(readBuffer, ref bufferIndex);
	//                globalColourTable[n][2] = ImageFileLoader.ReadByte(readBuffer, ref bufferIndex);
	//            }
	//        }

	//        int GRAPHIC_CONTROL_BLOCKCount = 0;
	//        int IMAGE_DESCRIPTORcount = 0;

	//        bool stop = false;
	//        int count = 0;
	//        while (stop == false)
	//        {
	//            ushort res1 = scanNextBlock();

	//            if (res1 == graphicControlExtLabel)
	//            {
	//                GRAPHIC_CONTROL_BLOCKCount++;

	//                if (GRAPHIC_CONTROL_BLOCKCount > (IMAGE_DESCRIPTORcount + 1))
	//                {
	//                    //error
	//                    return false;
	//                }
	//            }

	//            if (res1 == imageSeperator)
	//            {
	//                IMAGE_DESCRIPTORcount++;

	//                //even out missing graphic control extension and data
	//                if (IMAGE_DESCRIPTORcount > GRAPHIC_CONTROL_BLOCKCount)
	//                {
	//                    graphicControlArray.Add(new GRAPHIC_CONTROL_BLOCK());
	//                    graphicDataArray.Add(new GRAPHIC_CONTROL_DATA());
	//                }
	//            }

	//            //Once found processing is over for the loading.
	//            if (res1 == gifTrailer)
	//            {
	//                stop = true;
	//            }

	//            count++;

	//            //Safety.
	//            if (count >= MAX_FRAMES_ALLOWED)
	//            {
	//                stop = true;
	//            }
	//        }

	//        //Debug.Log("GRAPHIC_CONTROL_BLOCKCount " + GRAPHIC_CONTROL_BLOCKCount);
	//        //Debug.Log("IMAGE_DESCRIPTORcount " + IMAGE_DESCRIPTORcount);
	//        //Debug.Log("count " + count);

	//        for (int subIndex = 0; subIndex < IMAGE_DESCRIPTORcount; subIndex++)
	//        {
	//            byte[] dat = null;
	//            int datLen = 0;
	//            result = decompressB(compressedData[subIndex],
	//                        compressedDataLength[subIndex],
	//                        ref dat,
	//                        ref datLen,
	//                        (ushort)LZWminCodeSizeArray[subIndex]);

	//            if (dat != null)
	//            {
	//                //Add the data to the arrays.
	//                byte[] d = new byte[datLen];
	//                Array.Copy(dat, d, datLen);
	//                decompressedData.Add(d);
	//                //Debug.Log("datLen " + d.Length);
	//                //decompressedDataLength.Add(datLen);
	//                //Decompressed data must be retained so do not delete dat.
	//            }
	//        }
	//        return result;
	//    }

	//    ushort rootSize;//in bits
	//    bool initOK;

	//    ushort coloursUsed;
	//    ushort bitsPerPixel;
	//    ushort offsetOfClearCode;
	//    ushort offsetOfEndCode;

	//    ushort bitsInCode;
	//    uint byteLengthOfStream = 100000;//This will be the total buffer size available for each decompression.
	//    int byteIndexInStream;
	//    uint bitPositionInStream;

	//    uint byteSizeCode;//2 because it cant be more than 12 bit ie less the 2 bytes.
	//    uint usedBitsCode;//normally starts at 8 bits and grows if needed.

	//    uint byteSizeOld;
	//    uint usedBitsOld;

	//    uint byteSizeNewString;
	//    uint byteIndexNewString;

	//    byte[] characterStream;
	//    byte[] code;
	//    byte[] old;
	//    byte[] newString;

	//    private bool decompressB(byte[] bufferIn, uint lengthIn, ref byte[] bufferOut, ref int lengthOut, ushort initialRootCodeSize)
	//    {
	//        bool result = true;
	//        bool stop = false;
	//        //bool error = false;
	//        int counter = 0;

	//        //bool resetOldFlag = false;

	//        rootSize = initialRootCodeSize;

	//        reset();

	//        ushort inSize = 0;
	//        if (true == initOK)
	//        {
	//            //Calculate the needed size for the string table (not including special codes).
	//            if (initialRootCodeSize == 2)
	//            {
	//                inSize = 4;
	//            }
	//            else if (initialRootCodeSize == 3)
	//            {
	//                inSize = 8;
	//            }
	//            else if (initialRootCodeSize == 4)
	//            {
	//                inSize = 16;
	//            }
	//            else if (initialRootCodeSize == 5)
	//            {
	//                inSize = 32;
	//            }
	//            else if (initialRootCodeSize == 6)
	//            {
	//                inSize = 64;
	//            }
	//            else if (initialRootCodeSize == 7)
	//            {
	//                inSize = 128;
	//            }
	//            else if (initialRootCodeSize == 8)
	//            {
	//                inSize = 256;
	//            }
	//            else
	//            {
	//                inSize = 256;
	//            }

	//            initTable(inSize);
	//        }
	//        else
	//        {
	//            reset();
	//            result = false;
	//            return result;
	//        }

	//        bitsInCode = (ushort)(initialRootCodeSize + 1);

	//        getCode(bufferIn, lengthIn);
	//        bitPositionInStream += bitsInCode;

	//        ushort index = 0;
	//        byte firstChar = 0;
	//        index = System.BitConverter.ToUInt16(code, 0);
	//        //memcpy(&index, code, byteSizeCode);

	//        if (index != offsetOfClearCode)
	//        {
	//            byte[] buf0 = stringTable[index];
	//            //int len0 = sizeInBytesOfStringInTable[index];
	//            int src_offset = 0;
	//            ReadBytes(buf0, ref src_offset, ref characterStream, ref byteIndexInStream, buf0.Length);
	//            //byteIndexInStream += (uint)len0;
	//            old = code;
	//            //if (true == checkAndReallocateOutputStream(byteIndexInStream + (uint)len0))
	//            //{
	//            //    //memcpy((characterStream + byteIndexInStream), buf0, len0);
	//            //    ReadBytes(buf0,ref characterStream,ref byteIndexInStream,buf0.Length)
	//            //    byteIndexInStream += (uint)len0;

	//            //    //Place the current code in the old code.
	//            //    //memcpy(old, code, byteSizeCode);
	//            //}
	//            //else
	//            //{
	//            //    result = false;
	//            //    stop = true;
	//            //}
	//        }
	//        else
	//        {
	//            index = System.BitConverter.ToUInt16(code, 0);
	//            //memcpy((unsigned char*)&index,code,byteSizeCode);

	//            initTable(inSize);
	//            bitsInCode = (ushort)(initialRootCodeSize + 1);

	//            //then attempt to get the first code again.
	//            getCode(bufferIn, lengthIn);
	//            bitPositionInStream += bitsInCode;
	//            index = 0;
	//            firstChar = 0;

	//            index = System.BitConverter.ToUInt16(code, 0);
	//            //memcpy(&index,code,byteSizeCode);

	//            byte[] buf0 = stringTable[index];

	//            //int len0 = sizeInBytesOfStringInTable[index];
	//            int src_offset = 0;
	//            ReadBytes(buf0, ref src_offset, ref characterStream, ref byteIndexInStream, buf0.Length);
	//            //byteIndexInStream += (uint)len0;
	//            old = code;
	//            //if (true == checkAndReallocateOutputStream(byteIndexInStream + (uint)len0))
	//            //{
	//            //    //memcpy( (characterStream+byteIndexInStream),buf0,len0);
	//            //    byteIndexInStream += (uint)len0;

	//            //    //Place the current code in the old code.
	//            //    //memcpy(old,code,byteSizeCode);
	//            //}
	//            //else
	//            //{
	//            //    result = false;
	//            //    stop = true;
	//            //}
	//        }

	//        index = System.BitConverter.ToUInt16(code, 0);
	//        //memcpy((unsigned char*)&index,code,byteSizeCode);


	//        while (false == stop)
	//        {
	//            bool r1 = getCode(bufferIn, lengthIn);
	//            bitPositionInStream += bitsInCode;

	//            //memset((unsigned char*)&index,0,sizeof(unsigned short));
	//            //memcpy((unsigned char*)&index,code,byteSizeCode);
	//            index = System.BitConverter.ToUInt16(code, 0);
	//            //Test if the code was the clear code.
	//            if (index == offsetOfClearCode)
	//            {
	//                initTable(inSize);
	//                bitsInCode = (ushort)(initialRootCodeSize + 1);

	//                //then attempt to get the new first code.
	//                getCode(bufferIn, lengthIn);
	//                bitPositionInStream += bitsInCode;
	//                index = 0;
	//                firstChar = 0;
	//                index = System.BitConverter.ToUInt16(code, 0);
	//                //memcpy(&index,code,byteSizeCode);

	//                byte[] buf0 = stringTable[index];
	//                int src_offset = 0;
	//                ReadBytes(buf0, ref src_offset, ref characterStream, ref byteIndexInStream, buf0.Length);
	//                old = code;

	//                //int len0 = sizeInBytesOfStringInTable[index];

	//                //if (true == checkAndReallocateOutputStream((uint)(byteIndexInStream + (uint)len0)))
	//                //{
	//                //    //memcpy( (characterStream+byteIndexInStream),buf0,len0);
	//                //    byteIndexInStream += (uint)len0;

	//                //    //Place the current code in the old code.
	//                //    //memcpy(old,code,byteSizeCode);
	//                //}
	//                //else
	//                //{
	//                //    result = false;
	//                //    stop = true;
	//                //}
	//            }
	//            else if (index == offsetOfEndCode)
	//            {
	//                stop = true;
	//            }
	//            else
	//            {
	//                //Dose the code exist in the table.
	//                if (index < stringTable.Count)
	//                {
	//                    //Exists in table.
	//                    //Ouput the string for code.
	//                    index = 0;
	//                    index = System.BitConverter.ToUInt16(code, 0);
	//                    //memcpy(&index,code,byteSizeCode);
	//                    byte[] buf = stringTable[index];
	//                    int len = buf.Length;
	//                    firstChar = buf[0];
	//                    //memcpy(&firstChar,buf,sizeof(unsigned char));
	//                    //Debug.Log("characterStream.Length " + characterStream.Length);
	//                    int src_offset = 0;
	//                    ReadBytes(buf, ref src_offset, ref characterStream, ref byteIndexInStream, len);
	//                    index = 0;
	//                    index = System.BitConverter.ToUInt16(old, 0);
	//                    //Debug.Log("index " + index);
	//                    //Debug.Log("stringTable.Count " + stringTable.Count);
	//                    byte[] buf2 = stringTable[index];
	//                    byte[] str = new byte[buf2.Length + 1];

	//                    if (str != null)
	//                    {
	//                        int dest_offset = 0;
	//                        src_offset = 0;
	//                        ReadBytes(buf2, ref src_offset, ref str, ref dest_offset, buf2.Length);
	//                        str[dest_offset] = firstChar;
	//                        //memcpy(str,buf2,len2);
	//                        //memcpy( (str+len2),&firstChar,sizeof(unsigned char));

	//                        //Add it to the table.
	//                        stringTable.Add(str);
	//                        //sizeInBytesOfStringInTable.Add(str.Length);

	//                        //Set old
	//                        //memset(old,0,byteSizeCode);
	//                        //memcpy(old,code,byteSizeCode);
	//                        old = code;
	//                    }

	//                    //if (true == checkAndReallocateOutputStream(byteIndexInStream + (uint)len))
	//                    //{
	//                    //    //Proceed with outputting the data.
	//                    //    //memcpy( (characterStream+byteIndexInStream),buf,len);
	//                    //    byteIndexInStream += (uint)len;

	//                    //    //Get the string for old to allow the new table
	//                    //    //entry to be made.
	//                    //    index = 0;
	//                    //    //memset(&index,0,sizeof(unsigned short));
	//                    //    //memcpy(&index,old,byteSizeOld);

	//                    //    byte[] buf2 = stringTable[index];

	//                    //    int len2 = sizeInBytesOfStringInTable[index];

	//                    //    //Make the new string.
	//                    //    byte[] str = new byte[len2 + 1];
	//                    //    if (str != null)
	//                    //    {
	//                    //        //memcpy(str,buf2,len2);
	//                    //        //memcpy( (str+len2),&firstChar,sizeof(unsigned char));

	//                    //        //Add it to the table.
	//                    //        stringTable.Add(str);
	//                    //        sizeInBytesOfStringInTable.Add(len2 + 1);

	//                    //        //Set old
	//                    //        //memset(old,0,byteSizeCode);
	//                    //        //memcpy(old,code,byteSizeCode);
	//                    //    }
	//                    //    else
	//                    //    {
	//                    //        stop = true;
	//                    //        result = false;
	//                    //    }
	//                    //}
	//                    //else
	//                    //{
	//                    //    result = false;
	//                    //    stop = true;
	//                    //}
	//                }
	//                else//---------------------------------
	//                {
	//                    //Does not exist but is next index.

	//                    //Get the string for old to allow the new table
	//                    //entry to be made.
	//                    index = 0;
	//                    index = System.BitConverter.ToUInt16(old, 0);
	//                    //memcpy(&index,old,byteSizeOld);
	//                    if (index < stringTable.Count)
	//                    {
	//                        byte[] buf = stringTable[index];
	//                        //int len = buf.Length;

	//                        firstChar = buf[0];
	//                        //memcpy(&firstChar,buf,sizeof(unsigned char));

	//                        //Make the new string.
	//                        byte[] str = new byte[(buf.Length + 1)];
	//                        if (str != null)
	//                        {
	//                            int dest_offset = 0;
	//                            int src_offset = 0;
	//                            ReadBytes(buf, ref src_offset, ref str, ref dest_offset, buf.Length);
	//                            //memcpy(str,buf,len);
	//                            //memcpy( (str+len),&firstChar,sizeof(unsigned char));
	//                            str[dest_offset] = firstChar;
	//                            //Add it to the table.
	//                            //len += 1;
	//                            stringTable.Add(str);
	//                            //sizeInBytesOfStringInTable.Add(len);

	//                            //Set old
	//                            //memset(old,0,byteSizeCode);
	//                            //memcpy(old,code,byteSizeCode);
	//                            old = code;
	//                            src_offset = 0;
	//                            ReadBytes(str, ref src_offset, ref characterStream, ref byteIndexInStream, str.Length);
	//                            //Output the string.
	//                            //if (true == checkAndReallocateOutputStream(byteIndexInStream + (uint)len))
	//                            //{
	//                            //    //Proceed with outputting the data.
	//                            //    //memcpy( (characterStream+byteIndexInStream),str,len);
	//                            //    byteIndexInStream += len;
	//                            //}
	//                            //else
	//                            //{
	//                            //    result = false;
	//                            //    stop = true;
	//                            //}
	//                        }
	//                        //else
	//                        //{
	//                        //    stop = true;
	//                        //    result = false;
	//                        //}
	//                    }
	//                    //else
	//                    //{
	//                    //    stop = true;
	//                    //    result = false;
	//                    //}
	//                }

	//                //Calculate if the last string added to the table was at an address (code)
	//                //equal to the value of (2**bitsInCode)-1
	//                //It is not neccasary to calculate but instead the know allowed values are few.
	//                //7,15,31,63,127,255,511,1023,2047,
	//                //should not be reached as the encoder should have cleared by this time 4095

	//                int lastIndex = (int)stringTable.Count - 1;
	//                if (
	//                    (3 == lastIndex) ||
	//                    (7 == lastIndex) ||
	//                    (15 == lastIndex) ||
	//                    (31 == lastIndex) ||
	//                    (63 == lastIndex) ||
	//                    (127 == lastIndex) ||
	//                    (255 == lastIndex) ||
	//                    (511 == lastIndex) ||
	//                    (1023 == lastIndex) ||
	//                    (2047 == lastIndex)
	//                    )
	//                {
	//                    //Increase the bits per pixel for the next read by one.
	//                    bitsInCode++;
	//                }
	//            }

	//            //Safety check to stop endless loops.
	//            if (bitPositionInStream >= (lengthIn * CHAR_BIT))
	//            {
	//                stop = true;
	//            }
	//            counter++;
	//        }

	//        bufferOut = characterStream;
	//        characterStream = null;
	//        lengthOut = byteIndexInStream;

	//        reset();

	//        return result;
	//    }

	//    //bool checkAndReallocateOutputStream(uint desiredLength)
	//    //{
	//    //    bool result = false;

	//    //    if (desiredLength <= byteLengthOfStream)
	//    //    {
	//    //        result = true;
	//    //    }
	//    //    else
	//    //    {
	//    //        byte[] temp = new byte[desiredLength];
	//    //        if (temp != null)
	//    //        {
	//    //            //memcpy(temp,characterStream,byteLengthOfStream);
	//    //            //delete characterStream;
	//    //            //characterStream=temp;
	//    //            //temp=0;
	//    //            byteLengthOfStream = desiredLength;
	//    //            result = true;

	//    //        }
	//    //        else
	//    //        {
	//    //            result = false;
	//    //        }
	//    //    }

	//    //    return result;
	//    //}

	//    private bool getCode(byte[] bufferIn, uint lengthIn)
	//    {
	//        bool result = false;
	//        uint readBytes = 0;
	//        uint bitOffset = 0;
	//        uint byteOffset = 0;
	//        bool used = false;

	//        //Calc the byte offset of the byte where the current bit index falls in.
	//        byteOffset = (bitPositionInStream / CHAR_BIT);

	//        //Check if it is across a boundary.
	//        bitOffset = (bitPositionInStream % CHAR_BIT);

	//        //Read the four bytes.
	//        if ((byteOffset + 4) <= lengthIn)
	//        {
	//            //Read from the byte offset.
	//            int offset = (int)byteOffset;
	//            readBytes = ImageFileLoader.ReadUInt(bufferIn, ref offset);
	//            //readBytes = ImageFileLoader.ReadUIntLittle(bufferIn, ref offset);
	//            //readBytes = System.BitConverter.ToUInt32(bufferIn,offset);
	//            //memcpy((unsigned char*)&readBytes,(bufferIn+byteOffset),sizeof(unsigned long));

	//            //Shift it down until correct bits are at lsb
	//            readBytes = (readBytes >> (int)bitOffset);
	//            result = true;
	//            used = true;
	//        }
	//        else
	//        {
	//            //Calc how many bytes left.
	//            uint rem = (byteOffset + 4) - lengthIn;
	//            //Read from the byte offset minus rem.
	//            int offset = (int)(byteOffset - rem);
	//            readBytes = ImageFileLoader.ReadUInt(bufferIn, ref offset);
	//            //memcpy((unsigned char*)&readBytes,(bufferIn+(byteOffset-rem)),sizeof(unsigned long));
	//            bitOffset += (8 * rem);

	//            //Shift it down until correct bits are at lsb
	//            readBytes = (readBytes >> (int)bitOffset);
	//            result = true;
	//            used = false;
	//        }

	//        //I now have  unsigned short with the data stored starting at the lsb.
	//        //I want to zero any values that are unused above the code size.
	//        uint mask = 0xffffffff;
	//        mask = (mask >> ((CHAR_BIT * 4) - bitsInCode));
	//        readBytes = readBytes & mask;

	//        //Output.
	//        ushort us = (ushort)readBytes;
	//        code = System.BitConverter.GetBytes(us);

	//        //memcpy(code,&readBytes,sizeof(unsigned short));
	//        usedBitsCode = bitsInCode;

	//        return result;
	//    }

	//    private void reset()
	//    {
	//        initOK = true;
	//        coloursUsed = 256;
	//        bitsPerPixel = 8;
	//        offsetOfClearCode = 257;
	//        offsetOfEndCode = 258;
	//        bitsInCode = 8;

	//        stringTable.Clear();
	//        //sizeInBytesOfStringInTable.Clear();

	//        byteIndexInStream = 0;
	//        bitPositionInStream = 0;

	//        characterStream = new byte[byteLengthOfStream];

	//        byteSizeCode = 2;
	//        usedBitsCode = 0;

	//        code = new byte[byteSizeCode];

	//        byteSizeOld = 2;
	//        usedBitsOld = 0;

	//        old = new byte[byteSizeOld];

	//        byteSizeNewString = 256;
	//        byteIndexNewString = 0;

	//        newString = new byte[byteSizeNewString];

	//        rootSize = 8;

	//        return;
	//    }

	//    private List<byte[]> stringTable;
	//    //private List<int> sizeInBytesOfStringInTable;

	//    private void initTable(ushort initialSize)
	//    {
	//        if (initialSize > 256)
	//        {
	//            initialSize = 256;
	//        }

	//        if (initialSize < 4)
	//        {
	//            initialSize = 4;
	//        }

	//        //clear any previous table.
	//        if (stringTable.Count > 0)
	//        {
	//            stringTable.Clear();
	//        }

	//        //sizeInBytesOfStringInTable.Clear();
	//        byte[] rootString = null;
	//        byte[] bpara = null;
	//        uint count = 0;
	//        for (uint i = 0; i < initialSize; i++)
	//        {
	//            rootString = new byte[1];

	//            //remember x86 little endianess allows this cast to address the significant byte.
	//            bpara = System.BitConverter.GetBytes(count);
	//            rootString[0] = bpara[0];
	//            //Debug.Log("rootString.Length " + rootString.Length);
	//            //Debug.Log("rootString " + rootString[0].ToString());
	//            stringTable.Add(rootString);
	//            //sizeInBytesOfStringInTable.Add(1);

	//            count++;
	//        }

	//        //Remember that is now possible to have exceeded the size of an unsigned char.
	//        //Two unsigned chars will need to be used at this point.

	//        //Calculate the clear code.====================
	//        offsetOfClearCode = (ushort)count;
	//        //Debug.Log("offsetOfClearCode " + offsetOfClearCode);
	//        rootString = new byte[2];

	//        //remember x86 little endianess allows this cast to address the significant byte.
	//        //memcpy(rootString,((unsigned char*)(&offsetOfClearCode)),sizeof(unsigned short));
	//        bpara = System.BitConverter.GetBytes(offsetOfClearCode);
	//        rootString = bpara;
	//        //Debug.Log("rootString[0] " + rootString[0].ToString());
	//        //Debug.Log("rootString[1] " + rootString[1].ToString());

	//        stringTable.Add(rootString);
	//        //sizeInBytesOfStringInTable.Add(2);

	//        count++;

	//        //Calculate the end of data code.==============
	//        offsetOfEndCode = (ushort)count;

	//        rootString = new byte[2];

	//        //remember x86 little endianess allows this cast to address the significant byte.
	//        //memcpy(rootString,((unsigned char*)(&offsetOfEndCode)),sizeof(unsigned short));
	//        bpara = System.BitConverter.GetBytes(offsetOfEndCode);
	//        rootString = bpara;
	//        //Debug.Log("rootString[0] " + rootString[0].ToString());
	//        //Debug.Log("rootString[1] " + rootString[1].ToString());

	//        stringTable.Add(rootString);
	//        //sizeInBytesOfStringInTable.Add(2);

	//        count++;

	//        return;
	//    }
	//    private ushort scanNextBlock()
	//    {
	//        ushort result = 0;
	//        byte intro = 0;
	//        intro = ImageFileLoader.ReadByte(readBuffer, ref bufferIndex);

	//        if (intro == gifTrailer)
	//        {
	//            return gifTrailer;
	//        }

	//        byte label = 0;
	//        label = ImageFileLoader.ReadByte(readBuffer, ref bufferIndex);

	//        bufferIndex -= 1;
	//        bufferIndex -= 1;

	//        if (intro == imageSeperator)
	//        {
	//            label = imageSeperator;
	//        }

	//        switch (label)
	//        {
	//            case 0x2C:
	//                {
	//                    result = imageSeperator;
	//                    imageDescriptorForCopy.Read(readBuffer, ref bufferIndex);

	//                    IMAGE_DESCRIPTOR imageDescript = new IMAGE_DESCRIPTOR();
	//                    IMAGE_DATA imageData = new IMAGE_DATA();

	//                    imageDescript = imageDescriptorForCopy;
	//                    imageDescriptorArray.Add(imageDescript);

	//                    imageData.x = (ushort)((imageDescript.xposh << 8) | (imageDescript.xposl << 0));
	//                    imageData.y = (ushort)((imageDescript.yposh << 8) | (imageDescript.yposl << 0));
	//                    imageData.w = (ushort)((imageDescript.widthh << 8) | (imageDescript.widthl << 0));
	//                    imageData.h = (ushort)((imageDescript.heighth << 8) | (imageDescript.heightl << 0));

	//                    byte tableFollowsMask = 0x80;//0b10000000
	//                    if ((imageDescript.packedFields & tableFollowsMask) == tableFollowsMask)
	//                    {
	//                        imageData.colourTableFollows = 1;
	//                    }
	//                    else
	//                    {
	//                        imageData.colourTableFollows = 0;
	//                    }

	//                    byte interlaceMask = 0x40;//0b01000000
	//                    if ((imageDescript.packedFields & interlaceMask) == interlaceMask)
	//                    {
	//                        imageData.imageIsInterlaced = 1;
	//                    }
	//                    else
	//                    {
	//                        imageData.imageIsInterlaced = 0;
	//                    }

	//                    byte sortMask = 0x40;//0b00100000
	//                    if ((imageDescript.packedFields & sortMask) == sortMask)
	//                    {
	//                        imageData.colourTableIsSorted = 1;
	//                    }
	//                    else
	//                    {
	//                        imageData.colourTableIsSorted = 0;
	//                    }

	//                    if (imageData.colourTableFollows == 0)
	//                    {
	//                        imageData.entriesInLocalColourTable = 0;
	//                    }
	//                    else
	//                    {
	//                        //Clean the other bits off by shifting.
	//                        byte size = (byte)(imageDescript.packedFields << 5);
	//                        size = (byte)(size >> 5);
	//                        imageData.entriesInLocalColourTable = size;
	//                    }

	//                    imageDataArray.Add(imageData);

	//                    if (imageData.colourTableFollows != 0)
	//                    {
	//                        byte[][] colourTable = new byte[MAX_COLOURS_IN_COLOUR_TABLE][];
	//                        for (int i = 0; i < MAX_COLOURS_IN_COLOUR_TABLE; i++)
	//                        {
	//                            colourTable[i] = new byte[3];
	//                        }
	//                        for (int n = 0; n < imageData.entriesInLocalColourTable; n++)
	//                        {

	//                            colourTable[n][0] = ImageFileLoader.ReadByte(readBuffer, ref bufferIndex);
	//                            colourTable[n][1] = ImageFileLoader.ReadByte(readBuffer, ref bufferIndex);
	//                            colourTable[n][2] = ImageFileLoader.ReadByte(readBuffer, ref bufferIndex);

	//                            //The standard for gif seems to have a bug in it with the
	//                            //total max size fror colour tables given as 767 bytes.
	//                            //This should be 768 so in this case the last byte will need to be removed.
	//                            /*if(n==255)
	//                            {
	//                                bufferIndex-=1;
	//                                colourTable[n][2]=0;
	//                            }*/
	//                        }
	//                        //Add the filled table.
	//                        localColourTableArray.Add(colourTable);
	//                    }
	//                    else
	//                    {
	//                        //Just add a blank.
	//                        localColourTableArray.Add(null);
	//                    }

	//                    byte LZWmin = new byte();
	//                    LZWmin = ImageFileLoader.ReadByte(readBuffer, ref bufferIndex);
	//                    LZWminCodeSizeArray.Add(LZWmin);

	//                    byte[] newDataBuffer = null;
	//                    uint newDataBufferLength = 0;
	//                    int tempBufferIndex = bufferIndex;
	//                    bool stop = false;

	//                    while (stop == false)
	//                    {
	//                        //Read the size byte.
	//                        byte sizeByte = new byte();
	//                        sizeByte = ImageFileLoader.ReadByte(readBuffer, ref tempBufferIndex);
	//                        newDataBufferLength += sizeByte;

	//                        //if greater than zero then continue
	//                        if (sizeByte > 0x00)
	//                        {
	//                            //pass the rest of the buffer
	//                            //Safety check.
	//                            if ((tempBufferIndex + (1 * sizeByte)) > readBufferLength)
	//                            {
	//                                return ushort.MaxValue;
	//                            }
	//                            tempBufferIndex += (sizeByte * 1);
	//                        }
	//                        else
	//                        {
	//                            //The last block with size 0 is the terminator.
	//                            stop = true;
	//                        }
	//                    };

	//                    if (newDataBufferLength > 0)
	//                    {
	//                        newDataBuffer = new byte[newDataBufferLength];
	//                        if (newDataBuffer == null)
	//                        {
	//                            return ushort.MaxValue;
	//                        }

	//                        int currentIndex = 0;
	//                        stop = false;
	//                        while (stop == false)
	//                        {
	//                            //Read the size byte.
	//                            byte sizeByte = new byte();
	//                            sizeByte = ImageFileLoader.ReadByte(readBuffer, ref bufferIndex);

	//                            //if greater than zero then continue
	//                            if (sizeByte > 0x00)
	//                            {
	//                                //memcpy( (newDataBuffer+currentIndex),(readBuffer+bufferIndex),(sizeByte*sizeof(unsigned char)));
	//                                //bufferIndex+=(sizeByte*sizeof(unsigned char));
	//                                //currentIndex+=(sizeByte*sizeof(unsigned char));
	//                                //int dest_offset = 0;
	//                                ReadBytes(readBuffer, ref bufferIndex, ref newDataBuffer, ref currentIndex, sizeByte * 1);
	//                                //currentIndex += sizeByte * 1;
	//                            }
	//                            else
	//                            {
	//                                //The last block with size 0 is the terminator.
	//                                stop = true;
	//                            }
	//                        };

	//                        compressedData.Add(newDataBuffer);
	//                        compressedDataLength.Add(newDataBufferLength);
	//                    }
	//                    else
	//                    {
	//                        compressedData.Add(null);
	//                        //decompressedData.Add((void*)NULL);
	//                        return ushort.MaxValue;
	//                    }
	//                }
	//                break;
	//            case 0xF9:
	//                {
	//                    result = graphicControlExtLabel;
	//                    graphicControl.Read(readBuffer, ref bufferIndex);

	//                    GRAPHIC_CONTROL_BLOCK grapExt = new GRAPHIC_CONTROL_BLOCK();
	//                    GRAPHIC_CONTROL_DATA graphExtData = new GRAPHIC_CONTROL_DATA();

	//                    grapExt = graphicControl;
	//                    graphicControlArray.Add(grapExt);

	//                    byte disposalMask = 0x1c;//0b00011100, 28
	//                    byte dis = 0;
	//                    dis = (byte)(disposalMask & grapExt.packedFields);
	//                    dis = (byte)(dis >> 2);
	//                    graphExtData.disposalMethod = dis;

	//                    byte userInputMask = 0x02;//0b00000010, 2
	//                    if ((grapExt.packedFields & userInputMask) == userInputMask)
	//                    {
	//                        graphExtData.userInput = 1;
	//                    }
	//                    else
	//                    {
	//                        graphExtData.userInput = 0;
	//                    }

	//                    byte transparentMask = 0x01;//0b00000001, 1
	//                    if ((grapExt.packedFields & transparentMask) == transparentMask)
	//                    {
	//                        graphExtData.isTransparency = 1;
	//                    }
	//                    else
	//                    {
	//                        graphExtData.isTransparency = 0;
	//                    }

	//                    graphExtData.indexOfTransparencyColourInTable = grapExt.transparentIndex;

	//                    graphExtData.delayForImage = (ushort)((grapExt.delayTimeh << 8) | (grapExt.delayTimel << 0));

	//                    graphicDataArray.Add(graphExtData);
	//                }
	//                break;
	//            case 0xFE:
	//                {
	//                    result = commentExtLabel;

	//                    COMMENT_EXTENSION ce = new COMMENT_EXTENSION();
	//                    ce.Read(readBuffer, ref bufferIndex);

	//                    bool stop = false;
	//                    while (stop == false)
	//                    {
	//                        //Read the size byte.
	//                        byte sizeByte = new byte();
	//                        sizeByte = ImageFileLoader.ReadByte(readBuffer, ref bufferIndex);
	//                        //if greater than zero then continue
	//                        if (sizeByte > 0x00)
	//                        {
	//                            bufferIndex += (sizeByte * 1);
	//                        }
	//                        else
	//                        {
	//                            //The last block with size 0 is the terminator.
	//                            stop = true;
	//                        }
	//                    };
	//                }
	//                break;
	//            case 0x01:
	//                {
	//                    result = plainTextExtLabel;

	//                    PLAIN_TEXT_EXTENSION ce = new PLAIN_TEXT_EXTENSION();

	//                    ce.Read(readBuffer, ref bufferIndex);

	//                    bool stop = false;
	//                    while (stop == false)
	//                    {
	//                        //Read the size byte.
	//                        byte sizeByte = new byte();
	//                        sizeByte = ImageFileLoader.ReadByte(readBuffer, ref bufferIndex);

	//                        //if greater than zero then continue
	//                        if (sizeByte > 0x00)
	//                        {
	//                            //Do nothing with the data as this class does not use it.
	//                            bufferIndex += (sizeByte * 1);
	//                        }
	//                        else
	//                        {
	//                            //The last block with size 0 is the terminator.
	//                            stop = true;
	//                        }
	//                    };
	//                }
	//                break;
	//            case 0xFF:
	//                {
	//                    result = applicationExtLabel;

	//                    //Read block in.
	//                    APPLICATION_EXTENSION ce = new APPLICATION_EXTENSION();
	//                    ce.Read(readBuffer, ref bufferIndex);

	//                    bool stop = false;
	//                    while (stop == false)
	//                    {
	//                        //Read the size byte.
	//                        byte sizeByte = new byte();
	//                        sizeByte = ImageFileLoader.ReadByte(readBuffer, ref bufferIndex);

	//                        //if greater than zero then continue
	//                        if (sizeByte > 0x00)
	//                        {
	//                            //Do nothing with the data as this class does not use it.
	//                            bufferIndex += (sizeByte * 1);
	//                        }
	//                        else
	//                        {
	//                            //The last block with size 0 is the terminator.
	//                            stop = true;
	//                        }
	//                    };
	//                }
	//                break;
	//            default:
	//                {
	//                    result = ushort.MaxValue;
	//                }
	//                break;
	//        }
	//        return result;
	//    }

	//    public Texture2D LoadToTexture(byte[] data)
	//    {
	//        int frameIndex = 0;

	//        Load(data);
	//        byte[] frameData = getFrameData(frameIndex);

	//        int widthStart = 0;
	//        int widthEnd = globalImageData.width;
	//        int heightStart = 0;
	//        int heightEnd = globalImageData.height;

	//        Debug.Log("widthEnd " + widthEnd);
	//        Debug.Log("heightEnd " + heightEnd);

	//        Texture2D texture = new Texture2D(widthEnd - widthStart, heightEnd - heightStart, TextureFormat.ARGB32, false);

	//        int offset = 0;
	//        for (int y = heightStart; y < heightEnd; ++y)
	//        {
	//            for (int x = widthStart; x < widthEnd; ++x)
	//            {
	//                byte r = ImageFileLoader.ReadByte(frameData, ref offset);
	//                byte g = ImageFileLoader.ReadByte(frameData, ref offset);
	//                byte b = ImageFileLoader.ReadByte(frameData, ref offset);
	//                byte a = 255;

	//                Color32 color = new Color32(r, g, b, a);
	//                texture.SetPixel(x > 0 ? x : -x, y > 0 ? y : -y, color);
	//            }
	//        }
	//        texture.Apply();
	//        return texture;
	//    }

	//    private byte[] getFrameData(int frameIndex)
	//    {
	//        if (frameIndex >= decompressedData.Count)
	//        {
	//            return null;
	//        }

	//        byte[] result = null;
	//        bool localSaved = false;
	//        byte[][] Gpalette = new byte[256][];
	//        for (int i = 0; i < 256; i++)
	//        {
	//            Gpalette[i] = new byte[3];
	//        }

	//        for (int k = 0; k < 255; k++)
	//        {
	//            Gpalette[k][0] = (byte)k; // red
	//            Gpalette[k][1] = (byte)k; // green
	//            Gpalette[k][2] = (byte)k; // blue
	//        }

	//        byte[][] palette = new byte[256][];
	//        for (int i = 0; i < 256; i++)
	//        {
	//            palette[i] = new byte[3];
	//        }

	//        for (int k = 0; k < 255; k++)
	//        {
	//            palette[k][0] = (byte)k; // red
	//            palette[k][1] = (byte)k; // green
	//            palette[k][2] = (byte)k; // blue
	//        }

	//        bool globalAvailable = false;
	//        if (globalImageData.globalColourTableFollows != 0)
	//        {
	//            globalAvailable = true;
	//        }


	//        bool localAvailable = false;
	//        if (imageDataArray[frameIndex].colourTableFollows != 0)
	//        {
	//            if (localColourTableArray.Count > frameIndex)
	//            {
	//                if (localColourTableArray[frameIndex] != null)
	//                {
	//                    localAvailable = true;
	//                }
	//            }
	//        }

	//        if (true == localAvailable)
	//        {
	//            //Fill from local table and save a copy in the last used palette
	//            byte[][] ctable = localColourTableArray[frameIndex];
	//            int offset = 0;
	//            for (byte j = 0; j < 255; j++)
	//            {
	//                palette[j][0] = ctable[offset][0]; // red
	//                palette[j][1] = ctable[offset][1]; // green
	//                palette[j][2] = ctable[offset][2]; // blue

	//                Gpalette[j][0] = ctable[offset][0]; // red
	//                Gpalette[j][1] = ctable[offset][1]; // green
	//                Gpalette[j][2] = ctable[offset][2]; // blue
	//                localSaved = true;

	//                offset += 3;
	//            }
	//        }
	//        else
	//        {
	//            if (true == globalAvailable)
	//            {
	//                //Fill from global table.
	//                for (byte j = 0; j < 255; j++)
	//                {
	//                    palette[j][0] = globalColourTable[j][0]; // red
	//                    palette[j][1] = globalColourTable[j][1]; // green
	//                    palette[j][2] = globalColourTable[j][2]; // blue
	//                }
	//            }
	//            else
	//            {
	//                if (true == localSaved)
	//                {
	//                    //Fill from saved colour table.
	//                    for (byte j = 0; j < 255; j++)
	//                    {
	//                        palette[j][0] = Gpalette[j][0]; // red
	//                        palette[j][1] = Gpalette[j][1]; // green
	//                        palette[j][2] = Gpalette[j][2]; // blue
	//                    }
	//                }
	//                else
	//                {
	//                    return null;
	//                }
	//            }
	//        }

	//        int offsetX = imageDataArray[frameIndex].x;
	//        int offsetY = imageDataArray[frameIndex].y;
	//        int width = imageDataArray[frameIndex].w;
	//        int height = imageDataArray[frameIndex].h;
	//        byte[] data = decompressedData[frameIndex];
	//        int length = data.Length;
	//        //Debug.Log("offsetX " + offsetX);
	//        //Debug.Log("offsetY " + offsetY);
	//        if ((width * height) != length)
	//        {
	//            return null;
	//        }
	//        Debug.Log("width " + width);
	//        Debug.Log("height " + height);
	//        Debug.Log("length " + length);
	//        result = new byte[length * 3];
	//        uint data_index = 0;
	//        uint result_index = 0;
	//        for (int y = 0; y < height; y++)
	//        {
	//            for (int x = 0; x < width; x++)
	//            {
	//                byte tableIndex = data[data_index++];
	//                result[result_index++] = palette[tableIndex][0];
	//                result[result_index++] = palette[tableIndex][1];
	//                result[result_index++] = palette[tableIndex][2];
	//            }
	//        }

	//        return result;
	//    }
	//}
}

