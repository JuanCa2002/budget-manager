function initializeTransactionForm(targetUrl) {
    $("#TransactionTypeId").change(async function () {
        const selectedValue = $(this).val();

        const answer = await fetch(targetUrl, {
            method: 'POST',
            body: selectedValue,
            headers: {
                'Content-Type': 'application/json'
            }
        });

        const json = await answer.json();
        const options = json.map(category => `<option value=${category.value}>${category.text}</option>`);

        $("#CategoryId").html(options);
    })
}