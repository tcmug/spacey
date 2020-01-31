extends TextureRect

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

onready var viewport = get_viewport()

func _ready():
	viewport.connect("size_changed", self, "window_resize");


func _process(delta):
	var current_size = OS.get_window_size()
	set_size(current_size)
	print(current_size)
