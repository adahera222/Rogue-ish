/*
JSONParse.js
A JSON Parser for the Unity Game Engine
 
Based on JSON_parse by Douglas Crockford
Ported by Philip Peterson (ironmagma/ppeterson)

Made usable on iOS (which requires strict typing) by Tonio Loewald
Also used more modern UnityScript features (put back the "switch" statements)
And fixed an error which prevented whitespace being used in one legitimate case
And consequently wrapped everything in a convenient container class

Usage:
var parsed : JSON = JSON.fromString( JSON_string );
var JSON_string = JSON.stringify();

var JSON_obj = JSON._object(); // new empty object
JSON_obj._set("key", JSON._string("value")); 
Debug.Log( JSON_obj.stringify() ); // {"key":"value"}

var JSON_array = JSON._array();
JSON_array._push(1)._push("two")._push( JSON._object()._set("foo","bar") );
Debug.Log( JSON_array.stringify() ); // [ 1, "two", {"foo":"bar"} ];
*/

#pragma strict

enum JSONType {
    _object = 0,
    _array = 1,
    _undefined = 2,
    _null = 3,
    _number = 4,
    _string = 5,
    _boolean = 6
}

class JSON {
    var type: JSONType = JSONType._undefined;
    var keys: String[];
    var values: JSON[];
    var number_value: double = 0;
    var string_value: String = "";
    var boolean_value: boolean = false;

    private static function error(JSON_obj: JSON, msg: String): void {
        if (JSON_obj) {
            msg += "\n" + JSON_obj.toString();
        }
        throw new System.Exception("JSON error -- " + msg);
    }

    static function fromString(s: String): JSON {
        return JSONParse.JSONParse(s);
    }

    function toString(): String {
        var s: String;
        var i: int;
        switch (type) {
            case JSONType._string:
            case JSONType._undefined:
            case JSONType._null:
            case JSONType._boolean:
                s = string_value;
                break;
            case JSONType._array:
                s = "[";
                for (i = 0; i < values.length; i++) {
                    s += i == 0 ? values[i].toString() : "," + values[i].toString();
                }
                s += "]";
                break;
            case JSONType._object:
                s = "{";
                for (i = 0; i < values.length; i++) {
                    s += i == 0 ? keys[i] + ": " + values[i].toString() : "," + keys[i] + ": " + values[i].toString();
                }
                s += "}";
                break;
            case JSONType._number:
                s = "" + number_value;
                break;
        }
        return s;
    }
    
    public static function JSON_encode(source:String) : String{
    	var s: String;
    	var i: int;
    	
		for (i = 0; i < source.length; i++) {
			switch (source[i]) {
				case '\n':
					s += "\\n";
					break;
				case "\f":
					s += "\\f";
					break;
				case "\\":
					s += "\\\\";
					break;
				case "/":
					s += "/";
					break;
				case "\r":
					s += "\\r";
				case "\t":
					s += "\\t";
				default:
					s += source[i];
			}
		}
		
		return s;
    }

    function stringify(): String {
        var s: String = "";
        var c: String;
        var i: int;
        switch (type) {
            case JSONType._string:
                s = "\"" + JSON_encode(string_value) + "\"";
                break;
            case JSONType._undefined:
            case JSONType._null:
            case JSONType._boolean:
                s = string_value;
                break;
            case JSONType._array:
                s = "[";
                for (i = 0; i < values.length; i++) {
                    s += i == 0 ? values[i].stringify() : "," + values[i].stringify();
                }
                s += "]";
                break;
            case JSONType._object:
                s = "{";
                for (i = 0; i < values.length; i++) {
                    s += i > 0 ? "," : "";
                    s += "\"" + JSON_encode(keys[i]) + "\":" + values[i].stringify();
                }
                s += "}";
                break;
            case JSONType._number:
                s = "" + number_value;
                break;
        }
        return s;
    }

    static function _undefined():JSON {
        var j = new JSON();
        j.string_value = "undefined";
        j.boolean_value = false;
        return j;
    }

    static function _array():JSON {
        var j = new JSON();
        j.type = JSONType._array;
        j.values = new JSON[0];
        return j;
    }

    function _push(val: JSON):JSON{
        var v: Array = new Array(values);
        v.push(val);
        values = v.ToBuiltin(JSON);
        return this;
    }
    
    function _push(d: double):JSON{
    	return _push( _number(d) );
    }
    
    function _push(s: String):JSON{
    	return _push( _string(s) );
    }

    static function _object(): JSON {
        var j = new JSON();
        j.type = JSONType._object;
        j.keys = new String[0];
        j.values = new JSON[0];
        return j;
    }

    function _set(key: String, value: JSON): JSON {
        if (type == JSONType._object) {
            for (var i = 0; i < keys.length; i++) {
                if (keys[i] === key) {
                    values[i] = value;
                    return;
                }
            }
            var k = new Array(keys);
            var v = new Array(values);
            k.push(key);
            v.push(value);
            keys = k.ToBuiltin(String);
            values = v.ToBuiltin(JSON);
        } else {
            error(this, "_set called on non-object");
        }
        return this;
    }
    
    function _set(key: String, d: double): JSON {
    	return _set( key, _number(d) );
    }
    
    function _set(key: String, s : String): JSON {
    	return _set( key, _string(s) );
    }
    
    function _get(key: String): JSON {
        if (type == JSONType._object) {
            for (var i = 0; i < keys.length; i++) {
                if (keys[i] == key) {
                    return values[i];
                }
            }
        }
        return _undefined(); // undefined!
    }
    
    function _get(index: int): JSON{
    	if (type == JSONType._array) {
    		if( index < values.length ){
    			return values[index];
    		} else {
    			error(this, "index out of bounds: " + index);
    		}
    	} else {
    		error(this, "not an array");
    	}
    	return _undefined();
    }

    static function _null(): JSON {
        var j = new JSON();
        j.string_value = "null";
        j.boolean_value = false;
        j.type = JSONType._null;
        return j;
    }

    static function _boolean(b: boolean): JSON {
        var j = new JSON();
        j.type = JSONType._boolean;
        j.boolean_value = b;
        j.string_value = b ? "true" : "false";
        j.number_value = b ? 1 : 0;
        return j;
    }

    static function _true(): JSON {
        return _boolean(true);
    }

    static function _false(): JSON {
        return _boolean(false);
    }

    static function _string(s: String): JSON {
        var j = new JSON();
        j.type = JSONType._string;
        j.string_value = s;
        return j;
    }

    static function _number(d: double): JSON {
        var j = new JSON();
        j.type = JSONType._number;
        j.number_value = d;
        return j;
    }

    static function _NaN(): JSON {
        var j = new JSON();
        j.type = JSONType._number;
        j.number_value = 0;
        return j;
    }
    
    static function test(){
		var s = "{ \"foo\": \"bar\", \"baz\" : [ 17, 18, 19, { \"fish\" : \"soup\" } ]}";
	
		Debug.Log( "JSONParse Unit Tests" );
		var j:JSON = JSON.fromString(s);
		Debug.Log( "tostring: " + j.toString() );
		Debug.Log( "stringified: " + j.stringify() );
	
		Debug.Log( "obj.foo: " + j._get("foo").toString() );
		Debug.Log( "obj.baz[2]: " + j._get("baz")._get(2).toString() );
		Debug.Log( "obj.baz[3].fish: " + j._get("baz")._get(3)._get("fish").toString() );
	
		var JSON_obj:JSON = JSON._object(); // new empty object
		JSON_obj._set("key", JSON._string("value")); 
		Debug.Log( JSON_obj.stringify() ); // {"key":"value"}

		var JSON_array = JSON._array();
		JSON_array._push(1)._push("two")._push( JSON._object()._set("foo","bar") );
		Debug.Log( JSON_array.stringify() ); // [ 1, "two", {"foo":"bar"} ];
	}
} /* END of JSON implementation */

private static var at: int;
private static var ch: String;
private static var escapee = {
    "\"": "\"",
    "\\": "\\",
    "/": "/",
    "b": "b",
    "f": "\f",
    "n": "\n",
    "r": "\r",
    "t": "\t"
};
private static var text: String;

private static function error(m): void {
    throw new System.Exception("SyntaxError: \nMessage: " + m +
        "\nAt: " + at +
        "\nText: " + text);
}

private static function next(c): System.String {
    if (c && c != ch) {
        error("Expected '" + c + "' instead of '" + ch + "'");
    }


    if (text.length >= at + 1) {
        ch = text.Substring(at, 1);
    } else {
        ch = "";
    }

    at++;
    return ch;

}

private static function next() {
    return next(null);
}

private static function number(): JSON {
    var number;
    var string = "";

    // Debug.Log("JSONParse number");

    if (ch == "-") {
        string = "-";
        next("-");
    }
    while (ch in ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"]) {
        string += ch;
        next();
    }
    if (ch == ".") {
        string += ".";
        while (next() && ch in ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"]) {
            string += ch;
        }
    }
    if (ch == "e" || ch == "E") {
        string += ch;
        next();
        if (ch == "-" || ch == "+") {
            string += ch;
            next();
        }
        while (ch in ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"]) {
            string += ch;
            next();
        }
    }
    number = Number.Parse(string);

    if (System.Double.IsNaN(number)) {
        return JSON._NaN();
    } else {
        return JSON._number(number);
    }

}

private static function string(): JSON {
    var hex: int;
    var i: int;
    var string: String = "";
    var uffff: int;

    // Debug.Log("JSONParse string");

    if (ch == "\"") {
        while (next()) {
            if (ch == "\"") {
                next();
                return JSON._string(string);
            } else if (ch == "\\") {
                next();
                if (ch == "u") {
                    uffff = 0;
                    for (i = 0; i < 4; i++) {
                        hex = System.Convert.ToInt32(next(), 16);
                        if (hex == Mathf.Infinity || hex == Mathf.NegativeInfinity) {
                            break;
                        }
                        uffff = uffff * 16 + hex;
                    }
                    var m: char = uffff;
                    string += m;
                } else if (ch in escapee) {
                    string += escapee[ch];
                } else {
                    break;
                }
            } else {
                string += ch;
            }
        }
    }
    error("Bad string");
    return JSON._undefined();
};

private static function white(): void {
    while (ch && (ch.length >= 1 && ch.Chars[0] <= 32)) { // if it's whitespace
        next();
    }
}

private static function value(): JSON {
    var obj: JSON;
    white();

    // Debug.Log("JSONParse value");

    switch (ch) {
        case "{":
            obj = object();
            break;
        case "[":
            obj = array();
            break;
        case "\"":
            obj = string();
            break;
        case "-":
            obj = number();
            break;
        default:
            obj = (ch in ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"]) ? number() : word();
    }

    return obj;
};

private static function word(): JSON {
    var obj: JSON;
    // Debug.Log("JSONParse word");

    switch (ch) {
        case "t":
            next("t");
            next("r");
            next("u");
            next("e");
            obj = JSON._true();
        case "f":
            next("f");
            next("a");
            next("l");
            next("s");
            next("e");
            obj = JSON._false();
        case "n":
            next("n");
            next("u");
            next("l");
            next("l");
            obj = JSON._null();
        default:
            obj = JSON._undefined();
    }

    error("Unexpected '" + ch + "'");
    return obj;
}

private static function array(): JSON {
    var j = JSON._array();

    // Debug.Log("JSONParse array");

    if (ch == "[") {
        next("[");
        white();
        if (ch == "]") {
            next("]");
            return j; // empty array
        }
        while (ch) {
            j._push(value());
            white();
            if (ch == "]") {
                next("]");
                return j;
            }
            next(",");
            white();
        }
    }
    error("Bad array");
    return JSON._undefined();
};

private static function object() {
    var key: JSON;
    var val: JSON;
    var object = JSON._object();

    // Debug.Log("JSONParse object");

    if (ch == "{") {
        next("{");
        white();
        if (ch == "}") {
            next("}");
            return object; // empty object
        }
        while (ch) {
            key = string();
            white();
            next(":");
            white();
            val = value();
            object._set(key.string_value, val);
            // Debug.Log( key.string_value + " = " + val.toString() );
            white();
            if (ch == "}") {
                next("}");
                return object;
            }
            next(",");
            white();
        }
    }
    
    error("Bad object");
    return JSON._undefined();
}

public static function JSONParse(source): JSON {
    var result: JSON;

    text = source;
    at = 0;
    ch = " ";
    result = value();
    white();
    if (ch) {
        error("Syntax error");
    }

    // Debug.Log("JSONParse -- parsed " + result.type + "\n" + result.toString());
    return result;
}
