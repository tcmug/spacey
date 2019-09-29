tool
extends EditorPlugin

func _enter_tree():
	# add_custom_type("MyButton", "Button", preload("MyButton.cs"), null)

	add_custom_type("BT_Base", "Node", preload("BT_Base.cs"), null)
	add_custom_type("BT_Sequence", "BT_Base", preload("BT_Sequence.cs"), null)
	add_custom_type("BT_Condition", "BT_Base", preload("BT_Condition.cs"), null)


func _exit_tree():
	pass
