using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Env
{

    public static Env instance = new();

    public string mongoUri = "";
    public string botKey = "";

    public string pepper = "";
}