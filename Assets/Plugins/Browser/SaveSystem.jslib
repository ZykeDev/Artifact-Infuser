
mergeInto(LibraryManager.library, {

  SaveGameJS: function(saveString) {
    if (saveString != null) {
        saveString = Pointer_stringify(saveString);
        localStorage.setItem("artinfSave", saveString);
    }
  },

  LoadGameJS: function() {
    var saveString = localStorage.getItem("artinfSave");
    
    if (saveString == null) {
        console.log("No savefile found.");
        return ""; 
    }

    console.log("Loaded " + saveString);

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