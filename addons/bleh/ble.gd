tool
extends EditorPlugin

func _enter_tree():
	add_custom_type("BT_Base", "Node", preload("BT_Base.cs"), null)
	add_custom_type("BT_Action", "Node", preload("BT_Action.cs"), null)
	add_custom_type("BT_Sequence", "Node", preload("BT_Sequence.cs"), null)
	add_custom_type("BT_Condition", "Node", preload("BT_Condition.cs"), null)
	add_custom_type("BT_Selector", "Node", preload("BT_Selector.cs"), null)

func _exit_tree():
	remove_custom_type("BT_Base")
	remove_custom_type("BT_Action")
	remove_custom_type("BT_Sequence")
	remove_custom_type("BT_Condition")
	remove_custom_type("BT_Selector")

