import React, { Component } from 'react'
import axios from 'axios'
import './TodoAddItem.css';

class TodoAddItem extends Component {
    render() {
        return (
            <React.Fragment>
                <form className="todo-add-item-form" name="todoAddItem" id="todoAddItem">
                    <input type="text" id="name" name="name" placeholder="Item name"></input>
                    <button type="submit" id="create" onClick={this.onCreate}>Create</button>
                </form>
            </React.Fragment>
        )
    }

    async onCreate(e) {
        e.preventDefault();

        var response = await axios.post('https://toodoapi220190926085529.azurewebsites.net/api/TodoItems',
            {
                name: e.target.document.forms.todoAddItem.name,
                isComplete: true
            }
        );
        if (response.status === 200) {
            console.log('ok');
        }
    }
}

export default TodoAddItem;