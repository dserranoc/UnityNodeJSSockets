let io = require('socket.io')(process.env.PORT || 52300);

// Custom classes
var Player = require('./Classes/Player');

console.log('Server has started');
var players = []
var sockets = []

io.on('connection', function(socket) {
    console.log('Connection Made!');

    var player = new Player();
    var thisPlayerID = player.id;

    players[thisPlayerID] = player;
    sockets[thisPlayerID] = socket;

    // Tell the client that this is our id for the server
    socket.emit('register', {id: thisPlayerID});
    socket.emit('spawn', player); // Tell myself I have spawned
    socket.broadcast.emit('spawn', player); // Tell the others I have spawned

    // Tell myself about everyone else in the game
    for (playerID in players){
        if (playerID != thisPlayerID){
            socket.emit('spawn', players[playerID]);
        }
    }

    // Positional Data from Client

    socket.on('updatePosition', function(data) {
        player.position.x = data.position.x;
        player.position.y = data.position.y;

        socket.broadcast.emit('updatePosition', player);
    });

    socket.on('disconnect', function() {
        console.log('A player has disconnected');
        delete players[thisPlayerID];
        delete sockets[thisPlayerID];
        socket.broadcast.emit('disconnected', player);
    })
});