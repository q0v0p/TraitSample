var device = new Device();
var sampleData = byte.MaxValue;

device.Connect(Connecter.USB2);
device.WriteDataFromOS(sampleData);
// "USB2.0に接続：通常データ書き込み"

device.Connect(Connecter.USB3);
device.WriteDataFromOS(sampleData);
// "USB3.0に接続：高速データ書き込み"

device.DisConnect();
device.WriteDataFromOS(sampleData);
// "USBが接続されていません"

class Device : IUSB3, IUSB2
{
    public Connecter Connecter { get; set; } = Connecter.None;
    public byte Data { get; set; } = byte.MinValue;
    public void Connect(Connecter connecter) => Connecter = connecter;
    public void DisConnect() => Connecter = Connecter.None;
    public void WriteDataFromOS(byte data)
    {
        var message = this switch
        {
            { Connecter: Connecter.USB2 } => ((IUSB2)this).NormalSpeedWriteData(byte.MaxValue),
            { Connecter: Connecter.USB3 } => ((IUSB3)this).HiSpeedWriteData(byte.MaxValue),
            { Connecter: Connecter.None } => "USBが接続されていません",
            _ => throw new Exception()
        };

        Console.WriteLine(message);
    }
}

interface IUSB2 : IUSB
{
    public byte NormalSpeedReadData() { return Data; }
    public string NormalSpeedWriteData(byte writeData) { Data = writeData; return "USB2.0に接続：通常データ書き込み"; }
}

interface IUSB3 : IUSB
{
    public byte HiSpeedReadData() { return Data; }
    public string HiSpeedWriteData(byte writeData) { Data = writeData; return "USB3.0に接続：高速データ書き込み"; }
}

interface IUSB
{
    public byte Data { get; set; }
}

enum Connecter
{
    USB2,
    USB3,
    None
}