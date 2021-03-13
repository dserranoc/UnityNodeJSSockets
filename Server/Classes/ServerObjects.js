var Vector2 = require("./Vector2");
var shortID = require("shortid");
module.exports = class ServerObject {
    constructor(){
        this.id = shortID.generate();
        this.name = 'ServerObject';
        this.position = new Vector2();
    }
}