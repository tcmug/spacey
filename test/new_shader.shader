shader_type canvas_item;
render_mode unshaded;

uniform vec4 color : hint_color;
uniform float frequency = 500;
uniform float depth = 0.005;

void fragment()
{
	//COLOR = color; 
	vec2 uv = UV;
	float freq = min(frequency, max(0, sin(uv.x + TIME) * 500.0));
	float dep = min(depth, max(0, cos(uv.y + TIME) * 0.01));
	
	uv.x += sin(uv.y * freq + TIME) * dep;
	uv.x = clamp(uv.x, 0.0, 1.0);
	
	COLOR.rgba = texture(TEXTURE, uv);
	/*
	int y = int(uv.y * 600.0 * 2.0);
	int x = int(uv.x * 1025.0 * 2.0);
	if ((y % 2) < 1 && (x % 2) < 1) {
		COLOR.rgba = texture(TEXTURE, uv);
	}
	else {
		COLOR.rgba = vec4(0, 0, 0, 0);
	}*/
	

}