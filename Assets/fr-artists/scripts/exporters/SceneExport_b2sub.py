import bpy

                # TYPE PROJECT DIR HERE! 
                # 🡻🡻🡻🡻🡻🡻🡻🡻🡻🡻🡻
ProjectDir_bl = r"C:\Users\Jayjay\OneDrive - University of Gloucestershire\personal\RadioactiveTenticles"
name = "suzannehead"


bpy.ops.export_scene.fbx(
    filepath= f"{ProjectDir_bl}/texture/import/low/low.fbx",
    collection="low",
    use_mesh_modifiers=True
)

bpy.ops.export_scene.fbx(
    filepath= f"{ProjectDir_bl}/texture/import/high/high.fbx",
    collection="high",
    use_mesh_modifiers=True
)

print("|| BPY: the script ran! ||")