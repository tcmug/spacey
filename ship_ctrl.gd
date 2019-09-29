extends RigidBody

export var acceleration = 4000
export var acceleration_dampening = 50
export var angular_acceleration = 5000
export var angular_dampening = 50

var phase = 0
var mouse_sensitivity = 0.2
var torque = Vector3()
var rot_effect = Vector3()

var rate_delay: float = 0.0
var fire_rate: float = 4.0

var bullet = preload("res://effects/bullet.tscn")
var bullets
var effects

func _ready():
	bullets = get_tree().get_root().get_node("Spatial").get_node("World").get_node("Bullets")
	effects = get_tree().get_root().get_node("Spatial").get_node("World").get_node("Effects")

func _physics_process(delta):

	if get_colliding_bodies().size() > 0:

		if not $AudioStreamPlayer.playing:
			print("impact")
			$AudioStreamPlayer.play()

	var force: Vector3 = Vector3(0,0,0)

	if Input.is_action_pressed("move_forward"):
		force.z += -1
	if Input.is_action_pressed("move_backward"):
		force.z += 1
	if Input.is_action_pressed("move_left"):
		force.x += -1
	if Input.is_action_pressed("move_right"):
		force.x += 1

	if Input.is_action_pressed("fire"):
		rate_delay += delta
		if rate_delay > 1 / fire_rate:
			var vforce: Vector3 = transform.basis.xform(Vector3(0, 0, -3))
			shoot(translation, vforce)
			rate_delay -= 1 / fire_rate

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

	if torque.y != 0:
		rot_effect.z += torque.y * delta * 0.2
		rot_effect.z = clamp(rot_effect.z, -0.3, 0.3)
		# torque.z = rot_effect.z
		$Model.rotation.z = rot_effect.z
	else:
		var a = -delta * 0.5
		if rot_effect.z < 0:
			a = delta * 0.5
		if abs(rot_effect.z) <= abs(a):
			rot_effect.z = 0
		else:
			rot_effect.z += a

		rot_effect.z = clamp(rot_effect.z, -0.3, 0.3)
		$Model.rotation.z = rot_effect.z


	if torque.x != 0:
		rot_effect.x += torque.x * delta
		rot_effect.x = clamp(rot_effect.x, -0.1, 0.1)
		# $Model.rotation.x = rot_effect.x

	var vtorque: Vector3 = transform.basis.xform(torque)
	apply_torque_impulse(vtorque * angular_acceleration * delta)
	apply_torque_impulse(angular_velocity * -angular_dampening)
	torque = Vector3(0,0,0)

func _input(event):
	if event is InputEventMouseMotion:
		var moved = event.relative * -mouse_sensitivity
		torque = torque + Vector3(moved.y, moved.x, 0)

#var hit = preload("res://effects/hit.tscn")

func shoot(origin: Vector3, normal: Vector3):

	var obj = bullet.instance()

	if not $Shoot.playing:
		$Shoot.play()

	#obj.apply_central_impulse(normal * 100)
	obj.movement_normal = normal
	obj.translation = origin + normal * 3
	obj.rotation = rotation
	bullets.add_child(obj)


