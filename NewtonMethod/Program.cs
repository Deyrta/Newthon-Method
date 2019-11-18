using System;

namespace NewtonMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            NonLinearSystems solve = new NonLinearSystems();
            solve.NewtonMethod();
            Console.ReadKey();
        }
    }

    class NonLinearSystems
    {
        const double eps = 1e-5;
        double[] x0 = new double[3] { 0.5, 1.5, 3.5 };  // початкові наближення
        double[,] jakobi = new double[3, 3];  // Матриця Якобі
        double[] x = new double[3]; // минулі значення х
        double[] dob = new double[3] { 0, 0, 0 }; // добуток оберненої матриці на вектор значень ф-ії
        double max;

        public void NewtonMethod()
        {
            int iter = 0;
            do
            {
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        jakobi[i, j] = MatrJacobi(x0, i, j); // генерування матриці Якобі

                print_vectr(x0);  // Вивід коренів
                Console.WriteLine("Iteration number - {0}", iter);
                Console.WriteLine("=================");

                for (int i = 0; i < 3; i++) // рахуєм добуток оберненої матриці на вектор значень ф-ії
                    for (int j = 0; j < 3; j++)
                        dob[i] += jakobi[i, j] * Function(x0,j) / DetOfMatrJacobi(x0);

                for (int i = 0; i < 3; i++) // метод Ньютона
                    x[i] = x0[i] - dob[i];

                for (int i = 0; i < 3; i++)
                    max = Math.Abs(x[i] - x0[i]);  // обрахування значення для порівняння с похибкою

                x0 = (double[])x.Clone();
                iter++;
            }
            while (iter <=50 && max>=eps);
        }

        private double DetOfMatrJacobi(double[] x) => 2*x[0]*x[0]*x[0]-2*x[2]*(2*x[1]+2*x[1]*x[1])-4*x[1]*(1+x[2])+2*x[2]*(x[0]+x[0]*x[2])+8*x[0]*x[1]-2*x[0]*x[1]*(1+x[1]);

        private void print_vectr(double[] vector)
        {
            for (int j = 0; j < 3; ++j)
                Console.WriteLine("x" + j + "=" + vector[j]);
        }

        private double Function(double[] x, int i)
        {
            switch(i)
            {
                case 0:
                    return x[0] * x[0] + x[1] * x[1] - x[2] * x[2] + 4;
                case 1:
                    return x[0] + x[0] * x[1] - 2 * x[2] + 3;
                case 2:
                    return x[0] + x[1] * x[1] + x[0] * x[2] - 8;
            }
            return 0;
        }

        private double MatrJacobi(double[] x, int i, int j)
        {
            switch (i)
            {
                case 0:
                    switch (j)
                    {
                        case 0:
                            return 2 * x[0] * (x[0] * x[0] + 4 * x[1]);
                        case 1:
                            return -2 * x[1] * (x[0] + x[0] * x[1] + 2 + 2 * x[2]);
                        case 2:
                            return -2 * x[2] * (2 * x[1] + 2 * x[1] * x[1] - x[0] - x[0] * x[2]);
                    }
                    break;
                case 1:
                    switch (j)
                    {
                        case 0:
                            return -1 * (1 + x[1]) * (2 * x[0] * x[1] + 4 * x[2] * x[1]);
                        case 1:
                            return x[0] * (2 * x[0] * x[0] + 2 * x[2] + x[2] * x[2] * 2);
                        case 2:
                            return 2 * (4 * x[0] * x[1] - 2 * x[1] - 2 * x[1] * x[2]);

                    }
                    break;
                case 2:
                    {
                        switch (j)
                        {
                            case 0:
                                return (1 + x[2]) * (-4 * x[1] + 2 * x[0] * x[2]);
                            case 1:
                                return -2 * x[1] * (-4 * x[0] + 2 * x[2] + 2 * x[1] * x[2]);
                            case 2:
                                return x[0] * (2 * x[0] * x[0] - 2 * x[1] - 2 * x[1] * x[1]);
                        }
                    }
                    break;
            }
            return 0;
        }
    }
}
