using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    public class DirtGPUManager : MonoBehaviour
    {
        public Camera cam;

        public RenderTexture squeegeePositionTex; // Where the squeegee is right now
        public RenderTexture cleanedBufferTex; // Intermediate
        public RenderTexture cleanedTex; // Where all the cleaned glass is
        public Material applySqueegeeCleaningMat;
        public Material copyTextureMat;

        int _CleaningPower = Shader.PropertyToID("_CleaningPower");

        void Start()
        {
            InitializeTextures();
        }

        void InitializeTextures()
        {
            // Initialize textures to black
            Graphics.Blit(Texture2D.blackTexture, cleanedBufferTex);
            Graphics.Blit(Texture2D.blackTexture, cleanedTex);
        }

        private void Update()
        {
            // Don't squeegee if we are moving quickly up or down
            if (ScaffoldMotor.UsingButtons)
                return;

            cam.Render();
            Blit(cleanedTex, cleanedBufferTex, copyTextureMat); // Move the current data in the buffer
            applySqueegeeCleaningMat.SetFloat(_CleaningPower, SqueegeeHead.CleaningPower); // Tell the shader how much we should clean
            Blit(squeegeePositionTex, cleanedTex, applySqueegeeCleaningMat); // Copy the current pos into the tex
        }

        // https://forum.unity.com/threads/forcing-graphics-blit-to-work-can-you-help-me.962373/
        public static void Blit(Texture source, RenderTexture dest, Material mat, int pass = 0, bool executeImmediately = true)
        {
            var original = RenderTexture.active;
            RenderTexture.active = dest;

            if (mat != null)
                mat.SetTexture("_MainTex", source);
            GL.PushMatrix();
            GL.LoadOrtho();

            if (mat != null)
                mat.SetPass(pass);
            // draw a quad over whole screen
            GL.Begin(GL.QUADS);
            GL.TexCoord2(0f, 0f); GL.Vertex3(0f, 0f, 0f);    /* note the order! */
            GL.TexCoord2(0f, 1f); GL.Vertex3(0f, 1f, 0f);    /* also, we need TexCoord2 */
            GL.TexCoord2(1f, 1f); GL.Vertex3(1f, 1f, 0f);
            GL.TexCoord2(1f, 0f); GL.Vertex3(1f, 0f, 0f);
            GL.End();
            GL.PopMatrix();
            if (executeImmediately)
                GL.Flush();
            RenderTexture.active = original;    /* restore */
        }
    }
}
