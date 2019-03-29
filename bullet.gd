extends RigidBody

# Declare member variables here. Examples:
# var a = 2
# var b = "text"
var ttl = 0

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func _physics_process(delta):
	if get_colliding_bodies().size() > 0:
		print("freed")
		#free()	

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	ttl += delta
	if ttl > 5:
		free()

