$("#btnCreate").click(function () {
    //
    var htmlBody = "<input type='text' class='form-control' id='categoryName' value='' required />";
    // Set label
    $('#modalTitle').html("New Category");
    $('#modalBody').html(htmlBody);
    $('#modalBtnOk').html("Save");
    $('#modalBtnCancel').html("Cancel");
    // Set up action
    $('#modalBtnCancel').click(function () {
        $('#myModal').modal('hide');
    });
    $('#modalBtnOk').click(function () {
        var categoryName = $('#categoryName').val();
        if (categoryName == "") {
            $('#categoryName').css("border", "1px solid red");
            return;
        }
        // reset
        $('#categoryName').css("border", "1px solid #ced4da");
        // Show prompt
        Swal.fire({
            title: "Are you sure?",
            icon: "question",
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            showCancelButton: true,
            confirmButtonText: "Save",
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                // save here
                $.post("/Shop/CategoryCreate",
                  {
                      categoryName: categoryName
                  },
                  function (data, status) {
                      if (data.code == 0) {
                          Swal.fire(data.message, "", "success").then((result) => {
                              if (result.isConfirmed) {
                                  window.location = "/Shop/Category";
                              }
                          });
                          $('#myModal').modal('hide');
                      }
                      else {
                          Swal.fire(data.message, "", "danger");
                      }
                  });
            }
        });

    });

    $('#myModal').modal('show');
});
function onEdit(id) {
    $.get("/Shop/CategoryGetById/" + id, function (data, status) {
        var category = data;
        //
        var htmlBody = "<input type='text' class='form-control' id='categoryName' value='" + category.categoryName + "' required />";
        // Set label
        $('#modalTitle').html("Update Category(" + category.categoryName + ")");
        $('#modalBody').html(htmlBody);
        $('#modalBtnOk').html("Update");
        $('#modalBtnCancel').html("Cancel");
        // Set up action
        $('#modalBtnCancel').click(function () {
            $('#myModal').modal('hide');
        });
        $('#modalBtnOk').click(function () {
            var categoryName = $('#categoryName').val();
            if (categoryName == "") {
                $('#categoryName').css("border", "1px solid red");
                return;
            }
            // reset
            $('#categoryName').css("border", "1px solid #ced4da");
            // assign new brand name
            category.categoryName = categoryName;
            // Show prompt
            Swal.fire({
                title: "Are you sure?",
                icon: "question",
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                showCancelButton: true,
                confirmButtonText: "Save",
            }).then((result) => {
                /* Read more about isConfirmed, isDenied below */
                if (result.isConfirmed) {
                    // save here
                    $.post("/Shop/CategoryUpdate",
                      category,
                      function (data, status) {
                          if (data.code == 0) {
                              Swal.fire(data.message, "", "success").then((result) => {
                                  if (result.isConfirmed) {
                                      window.location = "/Shop/Category";
                                  }
                              });
                              $('#myModal').modal('hide');
                          }
                          else {
                              Swal.fire(data.message, "", "danger");
                          }
                      });
                }
            });

        });

        $('#myModal').modal('show');
    });
}
function onDelete(id) {
    Swal.fire({
        title: "Are you sure?",
        icon: "question",
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        showCancelButton: true,
        confirmButtonText: "OK",
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            // save here
            $.post("/Shop/BrandDelete",
              {
                  id: id
              },
              function (data, status) {
                  if (data.code == 0) {
                      Swal.fire(data.message, "", "success").then((result) => {
                          if (result.isConfirmed) {
                              window.location = "/Shop/Brand";
                          }
                      });
                      $('#myModal').modal('hide');
                  }
                  else {
                      Swal.fire(data.message, "", "danger");
                  }
              });
        }
    });
}