shader_type canvas_item;

// 颜色参数 - 用于鳞片染色
uniform vec4 tint_color : source_color = vec4(0.86, 0.20, 0.20, 1.0);

// 染色强度
uniform float tint_strength : hint_range(0.0, 1.0) = 0.6;

// 线条简化参数
uniform int color_levels : hint_range(2, 16) = 8;
uniform float detail_reduce : hint_range(0.0, 1.0) = 0.2;

// 图层模式：0=显示全部，1=只显示背景，2=只显示龙龙（染色）
uniform int layer_mode : hint_range(0, 2) = 0;

void fragment() {
	// 采样原始纹理
	vec4 tex_color = texture(TEXTURE, UV);
	
	// 计算亮度
	float brightness = dot(tex_color.rgb, vec3(0.299, 0.587, 0.114));
	
	// 根据图层模式处理
	if (layer_mode == 1) {
		// 只显示背景（白色区域）
		if (brightness > 0.85) {
			COLOR = tex_color;
		} else {
			COLOR = vec4(0.0, 0.0, 0.0, 0.0);
		}
	} else if (layer_mode == 2) {
		// 只显示龙龙（非白色区域），并染色
		if (brightness > 0.85) {
			COLOR = vec4(0.0, 0.0, 0.0, 0.0);
		} else {
			// 颜色量化
			vec3 quantized = floor(tex_color.rgb * float(color_levels) + 0.5) / float(color_levels);
			vec3 simplified = mix(tex_color.rgb, quantized, detail_reduce);
			
			// 灰度值保持明暗
			float gray = dot(simplified, vec3(0.299, 0.587, 0.114));
			
			// 染色
			vec3 tinted = mix(simplified, tint_color.rgb * gray * 2.0, tint_strength);
			
			COLOR = vec4(tinted, tex_color.a);
		}
	} else {
		// 显示全部（原始图片）
		COLOR = tex_color;
	}
}
