bl_info = {
    "name": "Custom Mesh Processing",
    "author": "Gage Unruh w/ ChatGPT",
    "version": (1, 0),
    "blender": (3, 6, 5),
    "location": "View3D > Object",
    "description": "Separate objects by material, merge vertices, and separate by loose parts",
    "category": "Object",
}

import bpy

def separate_by_materials(obj):
    # Separate by materials
    bpy.ops.object.mode_set(mode='EDIT')
    bpy.ops.mesh.separate(type='MATERIAL')
    bpy.ops.object.mode_set(mode='OBJECT')

def merge_vertices(obj, distance=0.00001):
    # Merge vertices
    bpy.ops.object.mode_set(mode='EDIT')
    bpy.ops.mesh.select_all(action='SELECT')
    bpy.ops.mesh.remove_doubles(threshold=distance)
    bpy.ops.object.mode_set(mode='OBJECT')

def separate_by_loose_parts(obj):
    # Separate by loose parts
    bpy.ops.object.mode_set(mode='EDIT')
    bpy.ops.mesh.separate(type='LOOSE')
    bpy.ops.object.mode_set(mode='OBJECT')

def process_single_object(obj):
    bpy.context.view_layer.objects.active = obj
    obj.select_set(True)

    # Check number of materials
    if len(obj.data.materials) > 1:
        separate_by_materials(obj)
        # After separation, only the original object remains selected
    merge_vertices(obj)
    separate_by_loose_parts(obj)

    obj.select_set(False)

def process_objects(context):
    selected_objects = context.selected_objects.copy()
    bpy.ops.object.select_all(action='DESELECT')

    for obj in selected_objects:
        if obj.type == 'MESH':
            process_single_object(obj)

class CustomMeshProcessing(bpy.types.Operator):
    bl_idname = "object.custom_mesh_processing"
    bl_label = "Custom Mesh Processing"
    bl_options = {'REGISTER', 'UNDO'}

    def execute(self, context):
        process_objects(context)
        return {'FINISHED'}

def menu_func(self, context):
    self.layout.operator(CustomMeshProcessing.bl_idname)

def register():
    bpy.utils.register_class(CustomMeshProcessing)
    bpy.types.VIEW3D_MT_object.append(menu_func)

def unregister():
    bpy.utils.unregister_class(CustomMeshProcessing)
    bpy.types.VIEW3D_MT_object.remove(menu_func)

if __name__ == "__main__":
    register()
