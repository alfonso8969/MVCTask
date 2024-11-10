function AddNewTaskToList() {
    taskListViewModel.tasks.push(new TaskElementListViewModel({ id: 0, title: '' }));

    $("[name=task-title]").last().trigger("focusin");

}

async function manageFocusoutTaskTitle(task) {
    const title = task.title();
    if (!title) {
        taskListViewModel.tasks.pop();
        return;
    }

    const data = JSON.stringify(title);
    const response = await fetch(urlTasks, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (response.ok) {
        const json = await response.json();
        task.id(json.id);
    } else {
        handleErrorApi(response);
    }
}

async function getTasks() {
    taskListViewModel.loading(true);

    const response = await fetch(urlTasks, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })

    if (!response.ok) {
        handleErrorApi(response);
        return;
    }

    const json = await response.json();
    taskListViewModel.tasks([]);

    json.forEach(valor => {
        taskListViewModel.tasks.push(new TaskElementListViewModel(valor));
    });

    taskListViewModel.loading(false);

}

async function updateTasksOrder() {
    const ids = getTasksIds();
    await sendTaskIdsToBackend(ids);

    const sortArray = taskListViewModel.tasks.sorted(function (a, b) {
        return ids.indexOf(a.id().toString()) - ids.indexOf(b.id().toString());
    });

    taskListViewModel.tasks([]);
    taskListViewModel.tasks(sortArray);
}

function getTasksIds() {
    const ids = $("[name=task-title]").map(function () {
        return $(this).attr("data-id");
    }).get();
    return ids;
}

async function sendTaskIdsToBackend(ids) {
    let data = JSON.stringify(ids);
    await fetch(`${urlTasks}/sort`, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });
}

async function handleClickTask(task) {
    if (task.isNew()) {
        return;
    }

    const response = await fetch(`${urlTasks}/${task.id()}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (!response.ok) {
        handleErrorApi(response);
        return;
    }

    const json = await response.json();

    taskEditVM.id = json.id;
    taskEditVM.title(json.title);
    taskEditVM.description(json.description);

    modalEditTaskBootstrap.show();

}

async function handleChangeEditTask() {
    const obj = {
        id: taskEditVM.id,
        title: taskEditVM.title(),
        description: taskEditVM.description()
    };

    if (!obj.title) {
        return;
    }

    await editCompleteTask(obj);

    const index = taskListViewModel.tasks().findIndex(t => t.id() === obj.id);
    const task = taskListViewModel.tasks()[index];
    task.title(obj.title);
}

async function editCompleteTask(task) {
    const data = JSON.stringify(task);

    const response = await fetch(`${urlTasks}/${task.id}`, {
        method: 'PUT',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (!response.ok) {
        handleErrorApi(response);
        throw new Error("error");
    }
}

function tryDeleteTask(task) {
    modalEditTaskBootstrap.hide();

    confirmAction({
        callBackAcept: () => {
            deleteTask(task);
        },
        callbackCancel: () => {
            modalEditTaskBootstrap.show();
        },
        title: `${deleteAnwser} ${task.title()}?`
    })

}

async function deleteTask(task) {
    const idTask = task.id;

    const response = await fetch(`${urlTasks}/${idTask}`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (response.ok) {
        const index = getIndexTaskInEdit();
        taskListadoViewModel.tasks.splice(index, 1);
    }
}

function getIndexTaskInEdit() {
    return taskListViewModel.tasks().findIndex(t => t.id() == taskEditVM.id);
}

$(function () {
    $("#sortable").sortable({
        axis: 'y',
        stop: async function () {
            await updateTasksOrder();
        }
    })
})