Array.prototype.sum = function (prop) {
    var total = 0;
    for (var i = 0, _len = this.length; i < _len; i++) {
        total += parseInt(this[i][prop]);
    }

    return total
}

Array.prototype.getConflictIndex = function (item, prop, currentIndex) {

    var conflictIndex = -1;
    var lengthOfArray = this.length;
    nextIndex = currentIndex + 1;
    if (nextIndex < lengthOfArray)
    {
        for (var i = nextIndex ; i < lengthOfArray; i++) {
            conflictIndex = item[prop] == this[i][prop] ? i : -1;
            if (conflictIndex >=0)
                break;
        }
    }

    return conflictIndex;
}


String.prototype.toCamelCase = function (stopper, startIndex) {
    var retString = "";
    if (stopper) {
        var matches = this.split(stopper);
        if (matches.length > 1) {
            matches = matches.splice(1, matches.length);
        }

        if (matches.length > 0) {
            for (var i = 0; i < matches.length; i++) {
                matches[i] = matches[i].substr(0, 1).toLowerCase() + matches[i].substr(1);
            }
        }
    }

    if (matches.length === 1) {
        return matches.join();
    }
    else {
        return matches.join(".");
    }
}