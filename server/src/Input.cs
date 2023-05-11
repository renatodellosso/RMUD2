using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Input
{

    public InputMode mode;
    public string text, id;

    public Input(InputMode mode, string id, string text)
    {
        this.mode = mode;
        this.id = id;
        this.text = text;
    }
}
