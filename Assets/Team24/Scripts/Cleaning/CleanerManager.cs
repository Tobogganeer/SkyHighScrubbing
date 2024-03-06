using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    [RequireComponent(typeof(Camera))]
    public class CleanerManager : MonoBehaviour
    {
        Camera cam;

        public RenderTexture renderTarget; // Where the squeegee is right now
        public RenderTexture doubleBuffer; // Intermediate
        public RenderTexture maskTex; // Where all the cleaned glass is
        public Material copyMaterial;
        public Material blankCopy;

        void Start()
        {
            cam = GetComponent<Camera>();
            Graphics.Blit(Texture2D.blackTexture, doubleBuffer);
            Graphics.Blit(Texture2D.blackTexture, maskTex);
        }

        private void Update()
        {
            // Don't squeegee if we are moving quickly up or down
            if (ScaffoldMotor.UsingButtons)
                return;

            cam.Render();
            Blit(maskTex, doubleBuffer, blankCopy); // Move the current data in the buffer
            Blit(renderTarget, maskTex, copyMaterial); // Copy the current pos into the tex
        }

        // https://forum.unity.com/threads/forcing-graphics-blit-to-work-can-you-help-me.962373/
        public static void Blit(Texture source, RenderTexture dest, Material mat, int pass = 0, bool executeImmediately = true)
        {
            var original = RenderTexture.active;
            RenderTexture.active = dest;   /* or Graphics.SetRenderTarget(..) */

            if (mat != null)
                mat.SetTexture("_MainTex", source);
            GL.PushMatrix();
            GL.LoadOrtho();
            // activate the first shader pass (in this case we know it is the only pass)
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
