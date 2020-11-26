[System.Serializable]
public class PrivateNetwork
{
    public string PrivateNetworkAddress;
    public int PrivateNetworkMaxPlayer;

    public PrivateNetwork()
    {
        this.PrivateNetworkAddress = "";
        this.PrivateNetworkMaxPlayer = 0;
    }

    public PrivateNetwork(string _privateNetworkAddress, int _privateNetworkMaxPlayer)
    {
        this.PrivateNetworkAddress = _privateNetworkAddress;
        this.PrivateNetworkMaxPlayer = _privateNetworkMaxPlayer;
    }
}
