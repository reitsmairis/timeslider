using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Netherlands3D.TileSystem;
using System.Linq;
using UnityEngine.UI;

namespace Netherlands3D.TreeRoots
{
    public class ChangeYear : MonoBehaviour
    {
        private float year;
        public string layerName = "";
        public string basePath = "";
        public int maximumDistance = 3000;
        public List<Material> DefaultMaterialList = new List<Material>();
        public TileHandler tileHandler;
        private int[] years = { 2020, 2025, 2030, 2035, 2040, 2045, 2050, 2055, 2060 };
        List<BinaryMeshLayer> timeSliderLayers = new List<BinaryMeshLayer>();
        public Slider timeSlider;

        // Awake is called before the first frame update and before Start of BinaryMeshLayer
        void Awake()
        {
            // Create layers for each year
            CreateLayersForEachYear();

            // Create initial layer with first year
            year = years[0];
            AdjustYear(year);
        }

        private void CreateLayersForEachYear()
        {
            for (int i = 0; i < years.Length; i++)
            {
                year = years[i];
                var dataSets = new List<DataSet>();

                // Create new gameobject for layer and put beneath tilehandler
                var newGameObject = new GameObject();
                newGameObject.name = layerName + "_" + year.ToString();
                newGameObject.transform.SetParent(tileHandler.transform);

                // Add binarymeshlayer component to gameobject
                var newBinaryMeshLayer = newGameObject.AddComponent<BinaryMeshLayer>();
                dataSets.Add(new DataSet()
                {
                    path = basePath.Replace("{year}", year.ToString()),
                    maximumDistance = maximumDistance
                });
                newBinaryMeshLayer.Datasets = dataSets;
                newBinaryMeshLayer.isEnabled = false;
                newBinaryMeshLayer.DefaultMaterialList = DefaultMaterialList;

                // Add binarymeshlayer to layer memory
                timeSliderLayers.Add(newBinaryMeshLayer);
                tileHandler.AddLayer(newBinaryMeshLayer);
            }
        }

        // AdjustYear is called when slider is moved
        public void AdjustYear(float newYear)
        {
            newYear = years.OrderBy(x => Math.Abs(x - newYear)).First();

            if (years.Contains((int)newYear))
            {
                // Turn layer corresponding to slider year on and old layer off
                int a = Array.IndexOf(years, (int)newYear);
                int b = Array.IndexOf(years, (int)year);

                // Change layers if timeslider was moved
                timeSliderLayers[a].isEnabled = true;
                if (a != b)
                {
                    timeSliderLayers[b].isEnabled = false;
                }
                year = newYear;

                timeSlider.SetValueWithoutNotify(newYear);
            }
        }
        public void ChangeOuterAlpha(float alpha)
        {
            int c = Array.IndexOf(years, (int)year);

            //var currentColor = timeSliderLayers[c].DefaultMaterialList[0].color;
            //print(currentColor);
            //currentColor.a = alpha;
            //timeSliderLayers[c].DefaultMaterialList[0].color = currentColor;

            var currentFloat = timeSliderLayers[c].DefaultMaterialList[0].GetFloat("_BaseFloat");
            currentFloat = alpha;
            timeSliderLayers[c].DefaultMaterialList[0].SetFloat("_BaseFloat", currentFloat);

        }
    }
}