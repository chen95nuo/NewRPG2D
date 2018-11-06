namespace HiSocket {
    public interface IPackage {
        void Unpack (IByteArray bytes);
        void Pack (IByteArray bytes);
        void ResetPrivateKey();
    }
}
