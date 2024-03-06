using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    public class DirtGPUManager : MonoBehaviour
    {
        public Camera squeegeeCam;
        public Camera waterCam;

        [Space]
        public RenderTexture squeegeePositionTex; // Where the squeegee is right now
        public RenderTexture waterPositionTex; // Where water is right now
        public RenderTexture cleanedBufferTex; // Intermediate
        public RenderTexture cleanedTex; // Where all the cleaned glass is

        [Space]
        public Material applySqueegeeCleaningMat;
        public Material copyTextureMat;

        [Space]
        public MeshRenderer dirtDisplay;
        Material dirtMat;

        readonly int _CleaningPower = Shader.PropertyToID("_CleaningPower");
        readonly int _WaterCleaningMult = Shader.PropertyToID("_WaterCleaningMult");
        readonly int _CleanedBuffer = Shader.PropertyToID("_CleanedBuffer");
        readonly int _WaterTex = Shader.PropertyToID("_WaterTex");

        // For dirt material
        readonly int _CleanedMask = Shader.PropertyToID("_CleanedMask");
        bool copiedTextures;

        void Start()
        {
            InitializeTextures();
        }

        void InitializeTextures()
        {
            // Make our own local copies of the RenderTextures
            squeegeePositionTex = new RenderTexture(squeegeePositionTex);
            waterPositionTex = new RenderTexture(waterPositionTex);
            cleanedBufferTex = new RenderTexture(cleanedBufferTex);
            cleanedTex = new RenderTexture(cleanedTex);

            squeegeeCam.targetTexture = squeegeePositionTex;
            waterCam.targetTexture = waterPositionTex;

            dirtMat = dirtDisplay.material;
            dirtMat.SetTexture(_CleanedMask, cleanedTex);

            copiedTextures = true;

            // Initialize textures to black
            Graphics.Blit(Texture2D.blackTexture, cleanedBufferTex);
            Graphics.Blit(Texture2D.blackTexture, cleanedTex);
        }

        private void OnDestroy()
        {
            if (copiedTextures)
            {
                // Remove the tex from the camera to prevent errors
                squeegeeCam.targetTexture = null;
                waterCam.targetTexture = null;

                // Destroy the copies we made
                DestroyImmediate(squeegeePositionTex);
                DestroyImmediate(waterPositionTex);
                DestroyImmediate(cleanedBufferTex);
                DestroyImmediate(cleanedTex);

                // Destroy the material we copied too
                DestroyImmediate(dirtMat);
            }
        }

        private void Update()
        {
            // Don't squeegee if we are moving quickly up or down
            if (ScaffoldMotor.UsingButtons)
                return;

            squeegeeCam.Render();
            Blit(cleanedTex, cleanedBufferTex, copyTextureMat); // Move the current data in the buffer

            applySqueegeeCleaningMat.SetFloat(_CleaningPower, SqueegeeHead.CleaningPower); // Tell the shader how much we should clean
            applySqueegeeCleaningMat.SetFloat(_WaterCleaningMult, SqueegeeHead.WaterCleaningMultiplier); // Tell the shader how much water helps
            applySqueegeeCleaningMat.SetTexture(_CleanedBuffer, cleanedBufferTex); // Set our copy of the cleaned buffer texture
            applySqueegeeCleaningMat.SetTexture(_WaterTex, waterPositionTex);
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
