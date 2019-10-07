namespace Parsing
{
    public interface IRsuBar
    {
        int ID { get; set; }
        double Mk { get; set; }
        double My { get; set; }
        double Mz { get; set; }
        double N { get; set; }
        int NumFe { get; set; }
        int NumSect { get; set; }
        double Qy { get; set; }
        double Qz { get; set; }
    }
}