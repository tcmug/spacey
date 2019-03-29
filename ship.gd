extends RigidBody

export var acceleration = 4000
export var acceleration_dampening = 50
export var angular_acceleration = 5000
export var angular_dampening = 50

var phase = 0
var mouse_sensitivity = 0.2
var torque = Vector3()

var bullet = preload("res://bullet.tscn")
var bullets

func _ready():
	bullets = get_tree().get_root().get_node("Spatial").get_node("Bullets")
	pass

func _physics_process(delta):

	var force: Vector3 = Vector3(0,0,0)

	if Input.is_action_pressed("move_forward"):
		force.z += -1
	if Input.is_action_pressed("move_backward"):
		force.z += 1
	if Input.is_action_pressed("move_left"):
		force.x += -1
	if Input.is_action_pressed("move_right"):
		force.x += 1

	var dir_force: Vector3 = transform.basis.xform(force)

	apply_central_impulse(dir_force * acceleration * delta)
	var lin_vel: Vector3 = linear_velocity.normalized()
	var remainder = lin_vel - dir_force.normalized()
	apply_central_impulse(remainder * -acceleration_dampening)

	var vforce: Vector3 = transform.basis.xform(Vector3(0, sin(phase) * 50, 0))
	apply_central_impulse(vforce)
	phase = phase + delta * 5

	if Input.is_action_pressed("rotate_left"):
		torque.z += 1
	if Input.is_action_pressed("rotate_right"):
		torque.z += -1

	var vtorque: Vector3 = transform.basis.xform(torque)
	apply_torque_impulse(vtorque * angular_acceleration * delta)
	apply_torque_impulse(angular_velocity * -angular_dampening)
	torque = Vector3(0,0,0)

func _input(event):
	if event is InputEventMouseMotion:
		var moved = event.relative * -mouse_sensitivity
		torque = torque + Vector3(moved.y, moved.x, 0)

	if event is InputEventMouseButton:
		var obj = bullet.instance()
		var vforce: Vector3 = transform.basis.xform(Vector3(0, 0, -1))
		obj.apply_central_impulse(vforce * 100)
		obj.translation = translation + vforce * 3
		obj.rotation  = rotation
		bullets.add_child(obj)
