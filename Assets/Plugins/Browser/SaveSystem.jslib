
mergeInto(LibraryManager.library, {

  SaveGame: function(saveString) {
    saveString = Pointer_stringify(saveString);

    console.log("Saving");
    console.log(saveString);
    console.log("------------------");

    localStorage.setItem("artinfSave", saveString);
  },

  LoadGame: function() {
    console.log("Loading game from " + "artinfSave");

    var saveString = localStorage.getItem("artinfSave");
    console.log("Loading");
    console.log(saveString);
    console.log("------------------");

    var bufferSize = lengthBytesUTF8(saveString) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(saveString, buffer, bufferSize);

    return buffer;
  },

  ClearSave: function() {
    localStorage.setItem("artinfSave", "");
    console.log("Save cleared.");
  }

});