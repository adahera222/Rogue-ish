Shader "Custom/ZMask" {
	Properties {
		//_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue"="Geometry-50" "IgnoreProjector"="True" "RenderType"="Opaque" }
		LOD 100
		
		Pass {
			ColorMask 0
		}
	} 
	//FallBack "Diffuse"
}
