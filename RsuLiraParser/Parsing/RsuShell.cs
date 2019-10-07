using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsing
{
    public class RsuShell
    {
        int iD;
        int numFe;
        int numSect;
        double nx, ny, txy, mx, my, mxy, qx, qy, rz;

        public int ID { get => iD; set => iD = value; }
        public int NumFe { get => numFe; set => numFe = value; }
        public int NumSect { get => numSect; set => numSect = value; }
        public double Nx { get => nx; set => nx = value; }
        public double Ny { get => ny; set => ny = value; }
        public double Txy { get => txy; set => txy = value; }
        public double Mx { get => mx; set => mx = value; }
        public double My { get => my; set => my = value; }
        public double Mxy { get => mxy; set => mxy = value; }
        public double Qx { get => qx; set => qx = value; }
        public double Qy { get => qy; set => qy = value; }
        public double Rz { get => rz; set => rz = value; }
    }
    public class RsuLShell
    {
        public int ID { get; set; }
        public int NumFe { get; set; }
        public int NumSect { get; set; }
        public double Nx { get; set; }
        public double Ny { get; set; }
        public double Txy { get; set; }
        public double Mx { get; set; }
        public double My { get; set; }
        public double Mxy { get; set; }
        public double Qx { get; set; }
        public double Qy { get; set; }
        public double Rz { get; set; }
    }
    public class RsuNShell
    {
        public int ID { get; set; }
        public int NumFe { get; set; }
        public int NumSect { get; set; }
        public double Nx { get; set; }
        public double Ny { get; set; }
        public double Txy { get; set; }
        public double Mx { get; set; }
        public double My { get; set; }
        public double Mxy { get; set; }
        public double Qx { get; set; }
        public double Qy { get; set; }
        public double Rz { get; set; }
    }
    public class RsuNLShell
    {
        public int ID { get; set; }
        public int NumFe { get; set; }
        public int NumSect { get; set; }
        public double Nx { get; set; }
        public double Ny { get; set; }
        public double Txy { get; set; }
        public double Mx { get; set; }
        public double My { get; set; }
        public double Mxy { get; set; }
        public double Qx { get; set; }
        public double Qy { get; set; }
        public double Rz { get; set; }
    }
}
