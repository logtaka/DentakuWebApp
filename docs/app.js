const state = {
    consoleElement: null,
    pendingResolvers: [],
    inputElementId: 'console-input-field'
};

function appendContent(message, addLineBreak) {
    if (!state.consoleElement) {
        return;
    }

    const lines = (message ?? '').split(/\n/);
    lines.forEach((line, index) => {
        const span = document.createElement('span');
        span.textContent = line;
        span.className = 'console-line';
        state.consoleElement.appendChild(span);

        if (addLineBreak || index < lines.length - 1) {
            state.consoleElement.appendChild(document.createElement('br'));
        }
    });

    state.consoleElement.scrollTop = state.consoleElement.scrollHeight;
}

export function initConsole(element) {
    state.consoleElement = typeof element === 'string'
        ? document.getElementById(element)
        : element;
}

export function write(message) {
    appendContent(message, false);
}

export function writeLine(message) {
    appendContent(message, true);
}

export function readLine() {
    return new Promise((resolve) => {
        state.pendingResolvers.push(resolve);
    });
}

export function sendInput(value) {
    appendContent(value, true);
    const resolver = state.pendingResolvers.shift();
    if (resolver) {
        resolver(value ?? '');
    }
}

export function focusInput() {
    const inputElement = document.getElementById(state.inputElementId);
    if (inputElement) {
        inputElement.focus();
    }
}

export const console = {
    initConsole,
    write,
    writeLine,
    readLine,
    sendInput,
};
