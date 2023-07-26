using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Input
{

    public InputMode mode;
    public string text, id;
    public bool selected, available;

    public Input(InputMode mode, string id, string text, bool selected = false, bool available = true)
    {
        this.mode = mode;
        this.id = id;
        this.text = text;
        this.selected = selected;
        this.available = available;
    }

    public Input(string id, string text, bool selected = false, bool available = false) : this(InputMode.Option, id, text, selected, available) { }
}
