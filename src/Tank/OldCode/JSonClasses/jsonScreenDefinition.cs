using System.Collections.Generic;

namespace Tank.Code.JSonClasses
{
    public class Button
    {
        public string Name
        {
            get; set;
        }
        public string Width
        {
            get; set;
        }
        public string Height
        {
            get; set;
        }
        public string VerticalAlignment
        {
            get; set;
        }
        public string HorizontalAlignment
        {
            get; set;
        }
        public int[] Margin
        {
            get; set;
        }
        public string Text
        {
            get; set;
        }
        public string TextureName
        {
            get; set;
        }
        public string HoverTextureName
        {
            get; set;
        }
        public string ClickEvent
        {
            get; set;
        }
        public string InternalData
        {
            get; set;
        }
    }

    public class Panel
    {
        public string Name
        {
            get; set;
        }
        public string Width
        {
            get; set;
        }
        public string Height
        {
            get; set;
        }
        public string VerticalAlignment
        {
            get; set;
        }
        public string HorizontalAlignment
        {
            get; set;
        }
        public int[] Margin
        {
            get; set;
        }
        public string TextureName
        {
            get; set;
        }
    }

    public class Textbox
    {
        public string Name
        {
            get; set;
        }
        public string Width
        {
            get; set;
        }
        public string Height
        {
            get; set;
        }
        public string TextAlignment
        {
            get; set;
        }
        public string VerticalAlignment
        {
            get; set;
        }
        public string HorizontalAlignment
        {
            get; set;
        }
        public int[] Margin
        {
            get; set;
        }
        public string TextureName
        {
            get; set;
        }
        public string ActiveTextureName
        {
            get; set;
        }
    }

    public class Textarea
    {
        public string Name
        {
            get; set;
        }
        public string Width
        {
            get; set;
        }
        public string Height
        {
            get; set;
        }
        public string TextAlignment
        {
            get; set;
        }
        public string VerticalAlignment
        {
            get; set;
        }
        public string HorizontalAlignment
        {
            get; set;
        }
        public int[] Margin
        {
            get; set;
        }
        public int[] Padding
        {
            get; set;
        }
        public string TextureName
        {
            get; set;
        }
        public string ActiveTextureName
        {
            get; set;
        }
        public string FontColor
        {
            get;
            set;
        }
        public string Text
        {
            get; set;
        }
    }

    public class Slider
    {
        public string Name
        {
            get; set;
        }
        public string ChangeEvent
        {
            get; set;
        }
        public string Width
        {
            get; set;
        }
        public string Height
        {
            get; set;
        }
        public string VerticalAlignment
        {
            get; set;
        }
        public string HorizontalAlignment
        {
            get; set;
        }
        public int[] Margin
        {
            get; set;
        }
        public string BackgroundTextureName
        {
            get; set;
        }
        public string SliderTextureName
        {
            get; set;
        }
        public int SliderWidth
        {
            get; set;
        }
        public int SliderHeight
        {
            get; set;
        }
        public int[] SliderOffset
        {
            get; set;
        }
    }

    public class Toggle
    {
        public string Name
        {
            get; set;
        }
        public string Width
        {
            get; set;
        }
        public string Height
        {
            get; set;
        }
        public string VerticalAlignment
        {
            get; set;
        }
        public string HorizontalAlignment
        {
            get; set;
        }
        public int[] Margin
        {
            get; set;
        }
        public string Text
        {
            get; set;
        }
        public string TextureName
        {
            get; set;
        }
        public string CheckedTextureName
        {
            get; set;
        }
        public string ClickEvent
        {
            get; set;
        }
    }

    public class jsonScreenDefinition
    {
        public string Name
        {
            get; set;
        }
        public List<Button> Buttons
        {
            get; set;
        }
        public List<Panel> Panels
        {
            get; set;
        }
        public List<Textbox> Textboxes
        {
            get; set;
        }
        public List<Textarea> Textareas
        {
            get; set;
        }
        public List<Slider> Sliders
        {
            get; set;
        }
        public List<Toggle> Toggles
        {
            get; set;
        }
        public string BackgroundTextureName
        {
            get; set;
        }
        public string BackgroundColor
        {
            get; set;
        }

        public jsonScreenDefinition()
        {
            Buttons = new List<Button>();
            Panels = new List<Panel>();
            Textboxes = new List<Textbox>();
            Textareas = new List<Textarea>();
            Sliders = new List<Slider>();
        }
    }
}
