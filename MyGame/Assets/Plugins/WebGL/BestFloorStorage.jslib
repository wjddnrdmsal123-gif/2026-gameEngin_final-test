mergeInto(LibraryManager.library, {
    SaveBestFloorJS: function (floor) {
        var key = "BestFloor";
        var currentBest = parseInt(localStorage.getItem(key) || "0");

        if (floor > currentBest) {
            localStorage.setItem(key, floor.toString());
            console.log("최고 층수 저장됨: F-" + floor);
        }
    },

    GetBestFloorJS: function () {
        var key = "BestFloor";
        var value = parseInt(localStorage.getItem(key) || "0");
        return value;
    },

    ResetBestFloorJS: function () {
        var key = "BestFloor";
        localStorage.removeItem(key);
        console.log("최고 층수 기록 초기화");
    }
});
