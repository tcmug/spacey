extends RayCast

var ttl = 5
var speed = 50
var movement_normal: Vector3
var hit = preload("res://effects/hit.tscn")
var effects

func _ready():
	effects = get_tree().get_root().get_node("Spatial").get_node("World").get_node("Effects")
	cast_to.z = -speed * 0.06
	pass

func _physics_process(delta):
	if is_colliding():

		var obj = hit.instance()
		
		var dir = movement_normal.reflect(get_collision_normal())
		obj.look_at(dir, Vector3(0,1,0))
		obj.translation = get_collision_point()
		effects.add_child(obj)
		obj.emitting = true
		free()
	else:
		translation += movement_normal * speed * delta




# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	ttl -= delta
	if ttl <= 0:
		free()

