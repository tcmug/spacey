extends TextureRect

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

onready var viewport = get_viewport()

func _ready():
	var rich = get_node("Terminal");
	viewport.connect("size_changed", self, "window_resize");
	window_resize();

func window_resize():
	var current_size = OS.get_window_size()
	set_size(current_size)
