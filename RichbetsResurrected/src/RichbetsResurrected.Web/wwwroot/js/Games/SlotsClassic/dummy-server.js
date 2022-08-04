const rollMin = 1
const rollMax = 9

var payoutMap = new Map()

function serverRedeem(id, hash) {
    console.log(`[CALL] Adding cash for ${id}`)
    payoutMap.delete(hash)
}

function serverSpin(value, id) {
    let randomSlot0 = Math.floor(Math.random() * (rollMax - rollMin + 1) + rollMin)
    let randomSlot1 = Math.floor(Math.random() * (rollMax - rollMin + 1) + rollMin)
    let randomSlot2 = Math.floor(Math.random() * (rollMax - rollMin + 1) + rollMin)

    let gameHash = makeId()
    let result = controlRollResult(randomSlot0, randomSlot1, randomSlot2, value, gameHash)

    if (result.isWin) {
        payoutMap.set(gameHash, id)
        controlMapTimer(id, gameHash)
    }
    return result
}

function controlMapTimer(id, hash) {
    setInterval(() => {
        if (payoutMap.has(hash)) {
            console.log(`[AUTOCALL] Adding cash for ${id}`)
            payoutMap.delete(hash)
        }
    }, 10000) // 10s
}

function controlRollResult(slot0, slot1, slot2, betValue, hash) {
    let winType;
    let isWin;

    if (slot0 === slot1 && slot1 === slot2) { // Full line
        if (slot0 === 9 && slot1 === 9 && slot2 === 9) { // Full line ` shooter ` max win
            winType = 15
            isWin = true
            return {
                result: [slot0, slot1, slot2],
                winType,
                isWin,
                betValue: betValue * winType,
                hash
            }
        }
        if (slot0 === 1 && slot1 === 1 && slot2 === 1) { // Full line ` 7 ` max win
            winType = 5
            isWin = true
            return {
                result: [slot0, slot1, slot2],
                winType,
                isWin,
                betValue: betValue * winType,
                hash
            }
        }
        winType = 3
        isWin = true
        return {
            result: [slot0, slot1, slot2],
            winType,
            isWin,
            betValue: betValue * winType,
            hash
        }
    }

    if (slot0 === slot1 || slot1 === slot2) { // 2 identical
        if (slot0 === 1 && slot1 === 1 || slot1 === 1 && slot2 === 1){ // 2 ` 7 ` inline
            winType = 2
            isWin = true
            return {
                result: [slot0, slot1, slot2],
                winType,
                isWin,
                betValue: betValue * winType,
                hash
            }
        }

        winType = 1.5
        isWin = true
        return {
            result: [slot0, slot1, slot2],
            winType,
            isWin,
            betValue: betValue * winType,
            hash
        }
    }

    winType = 0
    isWin = false
    return {
        result: [slot0, slot1, slot2],
        winType,
        isWin,
        betValue: betValue * winType,
        hash
    }
}

function makeId() {
    var result           = '';
    var characters       = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;
    for ( var i = 0; i < 10; i++ ) {
      result += characters.charAt(Math.floor(Math.random() * 
 charactersLength));
   }
   return result;
}