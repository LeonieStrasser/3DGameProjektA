//Maya ASCII 2023 scene
//Name: Ramp.ma
//Last modified: Tue, Oct 25, 2022 01:35:01 PM
//Codeset: 1252
requires maya "2023";
requires "mtoa" "5.1.0";
currentUnit -l centimeter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2023";
fileInfo "version" "2023";
fileInfo "cutIdentifier" "202202161415-df43006fd3";
fileInfo "osv" "Windows 10 Education v2009 (Build: 19044)";
fileInfo "UUID" "0C51D7E9-4374-8E89-E85E-1FA346E4AB04";
fileInfo "license" "education";
createNode transform -n "pCube2";
	rename -uid "DC36186C-45A4-ACB4-ECF8-66BFFCD052CB";
	setAttr ".s" -type "double3" 9.6782543470831275 9.6782543470831275 9.6782543470831275 ;
createNode mesh -n "pCubeShape2" -p "pCube2";
	rename -uid "DE06C49F-4487-B17C-F59F-BFB066F41133";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" nan nan ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 10 ".pt";
	setAttr ".pt[3]" -type "float3" 0 0.10483785 0 ;
	setAttr ".pt[5]" -type "float3" 0 0.10483785 0 ;
	setAttr ".pt[9]" -type "float3" 0 0.20896181 0 ;
	setAttr ".pt[11]" -type "float3" 0 0.20896181 0 ;
	setAttr ".pt[13]" -type "float3" 0.91371971 0 0 ;
	setAttr ".pt[14]" -type "float3" 0.91371971 0 0 ;
	setAttr ".pt[15]" -type "float3" 0.91371971 0 0 ;
	setAttr ".pt[16]" -type "float3" 0.91371971 0 0 ;
	setAttr ".dr" 1;
createNode mesh -n "polySurfaceShape1" -p "pCube2";
	rename -uid "9DC5E911-4E9C-58AA-DA36-06BEEDBA420B";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.25 0.375 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  1.6154536 0 0 1.6154536 0 
		0 0 0.97234643 0 1.6154536 -0.59194833 0.035385504 0 0.97234643 0 1.6154536 -0.59194833 
		-0.035385549 1.6154536 0 0 1.6154536 0 0;
	setAttr -s 8 ".vt[0:7]"  -0.5 -0.5 0.5 0.5 -0.5 0.5 -0.5 0.5 0.5 0.5 0.5 0.5
		 -0.5 0.5 -0.5 0.5 0.5 -0.5 -0.5 -0.5 -0.5 0.5 -0.5 -0.5;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode polyExtrudeFace -n "polyExtrudeFace1";
	rename -uid "E0121A1B-49AB-FA66-C5EF-3BACAF02382D";
	setAttr ".ics" -type "componentList" 1 "f[4]";
	setAttr ".ix" -type "matrix" 9.6782543470831275 0 0 0 0 9.6782543470831275 0 0 0 0 9.6782543470831275 0
		 0 0 0 1;
	setAttr ".ws" yes;
	setAttr ".pvt" -type "float3" 20.473902 -2.8645134 -8.6530338e-07 ;
	setAttr ".rs" 40641;
	setAttr ".c[0]"  0 1 1;
	setAttr ".cbn" -type "double3" 20.473901470016706 -4.8391271735415637 -5.1815969407683102 ;
	setAttr ".cbx" -type "double3" 20.473901470016706 -0.88989968896688465 5.1815952101615723 ;
	setAttr ".raf" no;
createNode polyTweak -n "polyTweak2";
	rename -uid "2ABBA855-4DEA-1493-C134-5EBB50B5229F";
	setAttr ".uopa" yes;
	setAttr -s 4 ".tk";
	setAttr ".tk[2]" -type "float3" 0 0 -0.2218094 ;
	setAttr ".tk[4]" -type "float3" 0 0 0.18033248 ;
createNode polyBevel3 -n "polyBevel1";
	rename -uid "BAF10723-4FF6-429D-33D6-A8A788142567";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 1 "e[*]";
	setAttr ".ix" -type "matrix" 9.6782543470831275 0 0 0 0 9.6782543470831275 0 0 0 0 9.6782543470831275 0
		 0 0 0 1;
	setAttr ".ws" yes;
	setAttr ".oaf" yes;
	setAttr ".f" 1;
	setAttr ".sg" 5;
	setAttr ".at" 180;
	setAttr ".sn" yes;
	setAttr ".mv" yes;
	setAttr ".mvt" 0.0001;
	setAttr ".sa" 30;
createNode polyTweak -n "polyTweak1";
	rename -uid "429A871D-4981-1419-2A51-17833ED59F84";
	setAttr ".uopa" yes;
	setAttr -s 9 ".tk";
	setAttr ".tk[2]" -type "float3" 0 0.36645767 0 ;
	setAttr ".tk[4]" -type "float3" 0 0.36645767 0 ;
	setAttr ".tk[8]" -type "float3" 0 -0.13655096 0 ;
	setAttr ".tk[9]" -type "float3" 0 -0.068198495 0 ;
	setAttr ".tk[10]" -type "float3" 0 -0.13236211 0 ;
	setAttr ".tk[11]" -type "float3" 0 0.36645767 0 ;
	setAttr ".tk[12]" -type "float3" 0 0.36645767 0 ;
	setAttr ".tk[13]" -type "float3" 0 -0.13236211 0 ;
createNode polyExtrudeVertex -n "polyExtrudeVertex1";
	rename -uid "84E94286-4769-A5C3-D3E3-D39DFBDF7DB3";
	setAttr ".ics" -type "componentList" 2 "vtx[2]" "vtx[4]";
	setAttr ".ix" -type "matrix" 9.6782543470831275 0 0 0 0 9.6782543470831275 0 0 0 0 9.6782543470831275 0
		 0 0 0 1;
	setAttr ".w" 0.5;
select -ne :time1;
	setAttr ".o" 1;
	setAttr ".unw" 1;
select -ne :hardwareRenderingGlobals;
	setAttr ".otfna" -type "stringArray" 22 "NURBS Curves" "NURBS Surfaces" "Polygons" "Subdiv Surface" "Particles" "Particle Instance" "Fluids" "Strokes" "Image Planes" "UI" "Lights" "Cameras" "Locators" "Joints" "IK Handles" "Deformers" "Motion Trails" "Components" "Hair Systems" "Follicles" "Misc. UI" "Ornaments"  ;
	setAttr ".otfva" -type "Int32Array" 22 0 1 1 1 1 1
		 1 1 1 0 0 0 0 0 0 0 0 0
		 0 0 0 0 ;
	setAttr ".fprt" yes;
select -ne :renderPartition;
	setAttr -s 2 ".st";
select -ne :renderGlobalsList1;
select -ne :defaultShaderList1;
	setAttr -s 5 ".s";
select -ne :postProcessList1;
	setAttr -s 2 ".p";
select -ne :defaultRenderingList1;
select -ne :initialShadingGroup;
	setAttr ".ro" yes;
select -ne :initialParticleSE;
	setAttr ".ro" yes;
select -ne :defaultRenderGlobals;
	addAttr -ci true -h true -sn "dss" -ln "defaultSurfaceShader" -dt "string";
	setAttr ".ren" -type "string" "arnold";
	setAttr ".dss" -type "string" "lambert1";
select -ne :defaultResolution;
	setAttr ".pa" 1;
select -ne :defaultColorMgtGlobals;
	setAttr ".cfe" yes;
	setAttr ".cfp" -type "string" "<MAYA_RESOURCES>/OCIO-configs/Maya2022-default/config.ocio";
	setAttr ".vtn" -type "string" "ACES 1.0 SDR-video (sRGB)";
	setAttr ".vn" -type "string" "ACES 1.0 SDR-video";
	setAttr ".dn" -type "string" "sRGB";
	setAttr ".wsn" -type "string" "ACEScg";
	setAttr ".otn" -type "string" "ACES 1.0 SDR-video (sRGB)";
	setAttr ".potn" -type "string" "ACES 1.0 SDR-video (sRGB)";
select -ne :hardwareRenderGlobals;
	setAttr ".ctrs" 256;
	setAttr ".btrs" 512;
connectAttr "polyExtrudeFace1.out" "pCubeShape2.i";
connectAttr "polyTweak2.out" "polyExtrudeFace1.ip";
connectAttr "pCubeShape2.wm" "polyExtrudeFace1.mp";
connectAttr "polyBevel1.out" "polyTweak2.ip";
connectAttr "polyTweak1.out" "polyBevel1.ip";
connectAttr "pCubeShape2.wm" "polyBevel1.mp";
connectAttr "polyExtrudeVertex1.out" "polyTweak1.ip";
connectAttr "polySurfaceShape1.o" "polyExtrudeVertex1.ip";
connectAttr "pCubeShape2.wm" "polyExtrudeVertex1.mp";
connectAttr "pCubeShape2.iog" ":initialShadingGroup.dsm" -na;
// End of Ramp.ma
