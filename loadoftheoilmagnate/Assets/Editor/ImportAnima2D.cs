using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MiniJSON;
using System.IO;

namespace SegaTechBlog
{
    public class ImportAnima2D : EditorWindow
    {

        [MenuItem("Window/Anima2D/Import Anima2D")]
        public static void Open()
        {

            // -----------------------------------------
            // �V�[���̏����擾
            // -----------------------------------------
            GameObject meshRoot = GameObject.Find("mesh");
            GameObject boneRoot = GameObject.Find("bone");

            // -----------------------------------------
            // �p�[�c�ʒuJSON�̏��ǂݏo��
            // -----------------------------------------
            string path_select = AssetDatabase.GetAssetPath(Selection.objects[0]);  // �I������Ă���Asset�̃p�X���擾
            string path_dir = Path.GetDirectoryName(path_select);     // �I��Asset�̃p�X����f�B���N�g�������o��
            string path_json = Directory.GetFiles(path_dir, "*-partspos.json")[0];    // ���f�B���N�g���̃p�[�c�ʒu Json ���擾
            var jsonText = File.ReadAllText(path_json);
            Dictionary<string, object> jsonData = MiniJSON.Json.Deserialize(jsonText) as Dictionary<string, object>;


            // -----------------------------------------
            // Project�őI�����Ă���Sprite��S�ĕ��ׂ�
            // -----------------------------------------
            foreach (Object asset in Selection.objects)
            {

                // -----------------------------------------
                // �p�[�c�ʒu���̓ǂݍ���
                // -----------------------------------------
                // JSON�f�[�^�̎��o��(miniJson)
                var jLayer = jsonData[asset.name] as Dictionary<string, object>;
                float index = System.Convert.ToSingle(jLayer["index"]);
                float px = System.Convert.ToSingle(jLayer["x"]);
                float py = System.Convert.ToSingle(jLayer["y"]);

                // -----------------------------------------
                // Anima2D�X�v���C�g�̔z�u
                // -----------------------------------------

                // Mesh
                GameObject tmpMesh = new GameObject(asset.name);
                var smeshInstance = tmpMesh.AddComponent<Anima2D.SpriteMeshInstance>();
                smeshInstance.sharedMaterial = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");
                tmpMesh.AddComponent<MeshFilter>();
                tmpMesh.AddComponent<MeshRenderer>();

                // Bone
                GameObject tmpBone = new GameObject("j_" + asset.name);
                Anima2D.Bone2D cmpBone = tmpBone.AddComponent<Anima2D.Bone2D>();
                tmpBone.transform.localRotation = Quaternion.Euler(0, 0, 90);

                // Sprite Mesh Instance �̐ݒ�
                smeshInstance.spriteMesh = asset as Anima2D.SpriteMesh; //  Mesh�̊��蓖��
                smeshInstance.bones = new List<Anima2D.Bone2D> { cmpBone };

                // Order in Layer �̐ݒ�
                smeshInstance.sortingOrder = (int)index;

                // Position �̐ݒ�
                tmpMesh.transform.position = new Vector3(px / 100.0f, py / 100.0f, 0.0f);
                tmpBone.transform.position = new Vector3(px / 100.0f, py / 100.0f, 0.0f);

                // �e�q�̐ݒ�
                tmpMesh.transform.parent = meshRoot.transform;
                tmpBone.transform.parent = boneRoot.transform;

            }

            // -----------------------------------------
            // Order in layer �̒l�ŕ��בւ�
            // -----------------------------------------
            Dictionary<int, string> dicOrder = new Dictionary<int, string>();
            foreach (Transform child in meshRoot.transform)
            {
                int order = child.GetComponent<Anima2D.SpriteMeshInstance>().sortingOrder;
                dicOrder[order] = child.name;
            }
            ArrayList keys = new ArrayList(dicOrder.Keys);
            keys.Sort();
            keys.Reverse();

            int count = 0;
            foreach (int i in keys)
            {
                meshRoot.transform.Find(dicOrder[i]).SetSiblingIndex(count);
                boneRoot.transform.Find("j_" + dicOrder[i]).SetSiblingIndex(count);
                count++;
            }

        }

    }

}
