namespace Parsing
{
    public interface IRsuShell
    {
        int ID { get; set; }
        double Mx { get; set; }
        double Mxy { get; set; }
        double My { get; set; }
        int NumFe { get; set; }
        int NumSect { get; set; }
        double Nx { get; set; }
        double Ny { get; set; }
        double Qx { get; set; }
        double Qy { get; set; }
        double Rz { get; set; }
        double Txy { get; set; }
    }
}