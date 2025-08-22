function onChangeRowsPerPage(baseUrl) {
    $("#rowsPerPage").change(function () {
        const rowsPerPage = $(this).val();
        location.href = `${baseUrl}?Page=1&RowsPerPage=${rowsPerPage}`;
    });
}