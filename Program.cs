using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entregable_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            int controller = 0;
            do
            {
                System.Console.Clear();
                Componente[] componentes = new Componente[] {
                    //tomando el dolar a 20 pesos 
                    new Componente(0,"Motor",400),
                    new Componente(1,"Carrocería", 300),
                    new Componente(2,"llantas", 20),
                    new Componente(3,"Adorno", 30)
                };
                foreach (Componente componente in componentes)
                {
                    Console.WriteLine(componente);
                }
                Empresa p1 = new Empresa();
                List<Componente> pedido = p1.TomarPedido(componentes);
                Proveedor prov = new Proveedor();
                Console.WriteLine("\n*******************************************************************");
                Console.WriteLine("\n\t Detalle del pedido");
                foreach (Componente componente in pedido)
                {
                    Console.WriteLine("\n" + componente + "CANTIDAD: " + componente.DblCantidad);
                }
                double subTotal = prov.CalcularSubtotal(pedido);
                Console.WriteLine("\n*******************************************************************");
                Console.WriteLine("El precio parcial antes de promociones es: $" + subTotal);
                double promocion = prov.CalcularPromociones(pedido, subTotal);
                double descuento = prov.CalcularDescuentos(pedido);
                double total = subTotal - promocion - descuento;
                Console.WriteLine("\n\tEl descuento por compra mayor a $20,000.00 es : $" + promocion);
                Console.WriteLine("\n\tEl descuento por compra de piezas mayor a 100 o 500 : $" + descuento);
                Console.WriteLine("\n*******************************************************************");
                Console.WriteLine("\n\t Costo Total");
                Console.WriteLine("\n\t $" + total);
                int cantidad = p1.Manufactura(pedido);
                Console.WriteLine("\n*******************************************************************");
                Console.WriteLine("La cantidad posible de cochecitos a ensamblar es: " + cantidad);
                Console.WriteLine("\n*******************************************************************");
                Console.WriteLine("\n\t Desea realizar otro presupuesto? Ingrese 1\n");
                controller = int.Parse(Console.ReadLine());
            } while (controller == 1);
        }
    }

    class Componente
    {
        private int intClave;
        private string strNombre;
        private double dblPrecio;
        private double dblCantidad;
        public int Clave { set => intClave = value; get => intClave; }
        public string Nombre { get => strNombre; set => strNombre = value; }
        public double Precio { get => dblPrecio; set => dblPrecio = value; }
        public double DblCantidad { get => dblCantidad; set => dblCantidad = value; }

        public Componente(int intClave, string strNombre, double dblPrecio)
        {
            this.intClave = intClave;
            this.strNombre = strNombre;
            this.dblPrecio = dblPrecio;
        }
        public Componente(Componente componente, double cantidad)
        {
            this.intClave = componente.Clave;
            this.strNombre = componente.Nombre;
            this.dblPrecio = componente.Precio;
            this.dblCantidad = cantidad;
        }
        public override string ToString()
        {
            return string.Format("componente: {0}, Clave: {1}, tiene un Precio de ${2}\n", strNombre, intClave, dblPrecio);
        }
    }

    class Empresa
    {
        public List<Componente> TomarPedido(Componente[] componentes)
        {
            int ctrl;
            List<Componente> pedido = new List<Componente>();
            do
            {
                Console.WriteLine("\n*******************************************************************");
                Console.WriteLine("Ingrese la clave que corresponda al componente que desea comprar: ");
                int opt = int.Parse(Console.ReadLine());
                if (opt < 4 && opt >= 0)
                {
                    Console.WriteLine("\nIngrese la cantidad que desea comprar: ");
                    double cant = double.Parse(Console.ReadLine());
                    Componente c = new Componente(componentes[opt], cant);
                    Console.WriteLine("\n");
                    pedido.Add(c);
                    foreach (Componente comp in pedido)
                    {
                        Console.WriteLine(comp.DblCantidad);
                    }
                    Console.WriteLine("\n Desea ingresar otro producto?  Escribe 1, de lo contrario ingresa cualquier otro valor.\n");
                    ctrl = int.Parse(Console.ReadLine());
                }
                else
                {
                    Console.WriteLine("Ingrese un valor valido");
                    ctrl = 1;
                }
            } while (ctrl == 1);
            return pedido;
        }

        public int Manufactura(List<Componente> inventario)
        {
            int cantidad;
            int clave;
            int numMotor = 0;
            int numCarroceria = 0;
            int numLlantas = 0;
            int numAdorno = 0;
            foreach (Componente c in inventario)
            {
                clave = c.Clave;
                switch (clave)
                {
                    case 0:
                        numMotor += (int)c.DblCantidad;
                        break;
                    case 1:
                        numCarroceria += (int)c.DblCantidad;
                        break;
                    case 2:
                        numLlantas += (int)c.DblCantidad;
                        break;
                    case 3:
                        numAdorno += (int)c.DblCantidad;
                        break;
                    default: break;
                }
            }
            numLlantas = numLlantas / 4;
            numAdorno = numAdorno / 2;
            if (numAdorno <= numLlantas && numMotor <= numCarroceria)
            {
                if (numAdorno <= numMotor)
                {
                    cantidad = numAdorno;
                }
                else cantidad = numMotor;
            }
            else if (numLlantas <= numCarroceria)
            {
                cantidad = numLlantas;
            }
            else cantidad = numCarroceria;
            return cantidad;
        }

    }

    class Proveedor
    {
        public double CalcularSubtotal(List<Componente> pedido)
        {
            double subTotal = 0;
            foreach (Componente c in pedido)
            {
                subTotal += c.DblCantidad * c.Precio;
            }
            return subTotal;
        }
        public double CalcularPromociones(List<Componente> pedido, double subTotal)
        {
            double promociones = 0;
            if (subTotal > 20000)
            {
                foreach (Componente c in pedido)
                {
                    if (c.Clave == 2 || c.Clave == 3)
                    {
                        int p = (int)(c.DblCantidad * 0.666666666);
                        int pDescuento = (int)(c.DblCantidad - p - 1);
                        Console.WriteLine("\n\tSe desconto: " + pDescuento + " piezas del producto: " + c.Nombre + "; promoción de 3x2");
                        promociones = (p * c.Precio);
                    }
                }
            }
            return promociones;
        }

        public double CalcularDescuentos(List<Componente> pedido)
        {
            double descuento = 0;
            double porcentaje;
            foreach (Componente c in pedido)
            {
                if (c.DblCantidad >= 500 && c.Clave == 1 || c.DblCantidad >= 500 && c.Clave == 0)
                {
                    porcentaje = 0.1;
                    double descuentoParcial = porcentaje * c.DblCantidad * c.Precio;
                    descuento += descuentoParcial;
                    Console.WriteLine("\nSe aplico un descuento del 10%, que equivale a : $" + descuentoParcial);
                }
                else if (c.DblCantidad >= 100 && c.Clave == 1 || c.DblCantidad >= 100 && c.Clave == 0)
                {
                    porcentaje = 0.05;
                    double descuentoParcial = porcentaje * c.DblCantidad * c.Precio;
                    descuento += descuentoParcial;
                    Console.WriteLine("\nSe aplico un descuento del 5%, que equivale a : $" + descuentoParcial);
                }
            }
            return descuento;
        }
    }
}
