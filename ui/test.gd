extends ViewportContainer

# Declare member variables here. Examples:
# var a = 2
# var b = "text"




var text = """VT232 FONT TEST
1.
2.
3.
	Tab
    Space
None
.
.
.
.
""";


func _ready():
	var rich = get_node("Viewport/Terminal");
	
var at = 0.0;
var delay = 0;
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var rich = get_node("Viewport/Terminal");
	
	if (text[at] == '\n' and delay < 1.0):
		delay += delta;
	else:
		delay = 0;
		rich.text = text.substr(0, ceil(at)) + "_";
		at += 0.5;
		if (at >= text.length()):
		  at = 0;
	