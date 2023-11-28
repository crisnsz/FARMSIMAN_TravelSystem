function arrayToJsonUser(inputArray) {
    // Define keys for the JSON object
    const jsonObject = {
        subsidiary_ID: inputArray[0],
        subsidiary_Name: inputArray[1],
        subsidiary_Address: inputArray[2],
        subsidiary_Distance: inputArray[3],
        subsidiary_Button: inputArray[4]
    };

    return jsonObject;
}