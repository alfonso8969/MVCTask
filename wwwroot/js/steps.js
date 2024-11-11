function handleClickAddStep() {

    const index = taskEditVM.steps().findIndex((step) => step.isNew());

    if (index !== -1) {
        return;
    }

    taskEditVM.steps.push(new StepViewModel({ editMode: true, isCompleted: false }));
    $("[name=txtStepDescription]:visible").trigger("focusin");
}

function handleClickCancelStep(step) {
    if (step.isNew()) {
        taskEditVM.steps.pop();
    } else {
        step.editMode(false);
        step.description(step.previousDescription);
    }
}

async function handleClickSaveStep(step) {
    step.editMode(false);
    const isNew = step.isNew();
    const idTask = taskEditVM.id;
    const data = getStepRequestBody(step);

    const description = step.description().trim();

    if (!description) {
        step.description(step.previousDescription);

        if (isNew) {
            taskEditVM.steps.pop();
        }
        return;
    }

    if (isNew) {
        await insertStep(step, data, idTask);
    } else {
        updateStep(data, step.id());
    }  
}

async function insertStep(step, data, idTask) {
    const response = await fetch(`${urlSteps}/${idTask}`, {
        method: "POST",
        body: data,
        headers: {
            "Content-Type": "application/json"
        }
    });
    if (response.ok) {
        const json = await response.json();
        step.id(json.id);

        const task = getTaskInEdit();
        task.totalSteps(task.totalSteps() + 1);

        if (step.isCompleted()) {
            task.completedSteps(task.completedSteps() + 1);
        }


    } else {
        handelErrorApi(response);
    }
}

function getStepRequestBody(step) {
    return JSON.stringify({
        description: step.description(),
        isCompleted: step.isCompleted()
    });
}
function handleClickStepDescription(step) {
    step.editMode(true);
    step.previousDescription = step.description();
    $("[name=txtStepDescription]:visible").trigger("focusin");
}

async function updateStep(data, id) {
    const response = await fetch(`${urlSteps}/${id}`, {
        method: "PUT",
        body: data,
        headers: {
            "Content-Type": "application/json"
        }
    });
    if (!response.ok) {
        handelErrorApi(response);
    }
}

function handleClickCheckboxStep(step) {
    if (step.isNew()) {
        return true;
    }

    const data = getStepRequestBody(step);
    updateStep(data, step.id());

    const task = getTaskInEdit();
    let actualCompletedSteps = task.completedSteps();

    if (step.isCompleted()) {
        actualCompletedSteps++;
    } else {
        actualCompletedSteps--;
    }
    task.completedSteps(actualCompletedSteps);

    return true;

}

function handleClickDeleteStep(step) {
    modalEditTaskBootstrap.hide();

    confirmAction({
        callBackAcept: () => {
            deleteStep(step);
            modalEditTaskBootstrap.show();          
        },
        callBackCancel: () => {
            modalEditTaskBootstrap.show();
        },
        title: deleteStepAnswer,
    })

}

async function deleteStep(step) {
    const response = await fetch(`${urlSteps}/${step.id()}`, {
        method: "DELETE"
    });
    if (!response.ok) {
        handelErrorApi(response);
        return;
    }
    taskEditVM.steps.remove((item) => item.id() == step.id());

    const task = getTaskInEdit();
    task.totalSteps(task.totalSteps() - 1);
    if (step.isCompleted()) {
        task.completedSteps(task.completedSteps() - 1);
    }
}

async function updateStepsOrder() {
    const ids = getIdsSteps();
    await sendIdsStepsToTheBackend(ids);

    const orderArray = taskEditVM.steps.sorted(function (a, b) {
        return ids.indexOf(a.id().toString()) - ids.indexOf(b.id().toString());
    })

    taskEditVM.steps(orderArray);
}

function getIdsSteps() {
    const ids = $("[name=chbStep]").map(function () {
        return $(this).attr('data-id')
    }).get();
    return ids;
}

async function sendIdsStepsToTheBackend(ids) {
    let data = JSON.stringify(ids);
    const response = await fetch(`${urlSteps}/sort/${taskEditVM.id}`, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (!response.ok) {
        handelErrorApi(response);
    }
}

$(function () {
    $("#sortableSteps").sortable({
        axis: 'y',
        stop: async function () {
            await updateStepsOrder();
        }
    })
})