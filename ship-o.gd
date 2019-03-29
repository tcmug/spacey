extends KinematicBody

var speed = 3
var velocity: Vector3 = Vector3()

func _ready():
	pass # Replace with function body.

func _physics_process(delta):
	
	var force: Vector3
	var torque: Vector3	
	
	if Input.is_action_pressed("ui_up"):
		force.z = -1
	elif Input.is_action_pressed("ui_down"):
		force.z = 1
	
	if Input.is_action_pressed("ui_right"):
		torque.y = -1
	elif Input.is_action_pressed("ui_left"):
		torque.y = 1
	
	var directional_force: Vector3 = transform.basis.xform(force)

	velocity = velocity + directional_force
#	apply_central_impulse(directional_force * speed * delta	)
#	apply_torque_impulse(torque * speed * delta)
			

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass



