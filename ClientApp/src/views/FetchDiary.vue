<template>
    <v-container>
        <div id="create" v-if="createPage">
            <v-content>
                <v-container
                        fill-height
                        fill-width
                >
                    <v-layout
                            align-start
                            justify-center
                    >
                        <v-flex
                                xs2
                                sm6
                                md8
                                lg10
                                xl12
                        >
                            <v-card class="elevation-12">
                                <v-toolbar
                                        color="primary"
                                        dark
                                        flat
                                >
                                    <v-toolbar-title>Create new page</v-toolbar-title>
                                    <v-spacer/>
                                    <v-btn icon @click.local="showCreationDialog">
                                        <v-icon>mdi-close</v-icon>
                                    </v-btn>
                                </v-toolbar>
                                <v-card-text>
                                    <v-form>
                                        <v-textarea  auto-grow v-model = "name" prepend-icon="book" placeholder="#Title#"/>
                                    </v-form>
                                </v-card-text>
                                    <v-card-text>
                                        <vue-markdown :source="name"/>
                                    </v-card-text>
                                <v-row justify="center" align ="center">
                                    <v-card-actions>
                                        <v-btn class="ma-2" color="primary" @click="create" >Create</v-btn>
                                    </v-card-actions>
                                </v-row>
                            </v-card>
                        </v-flex>
                    </v-layout>
                </v-container>
            </v-content>
        </div>
        <v-row v-if="editPage" xs2 sm6 md8 lg10 xl12>

        </v-row>
        <v-row v-if="editPage" xs2 sm6 md8 lg10 xl12>
            <v-col>
                <v-card>
                    <v-toolbar
                            color="primary"
                            dark
                            flat
                    >
                        <v-toolbar-title>Edit</v-toolbar-title>
                    </v-toolbar>
                        <v-textarea id="sel" auto-grow v-model = "currentPage" placeholder="#Title"/>
                </v-card>
            </v-col>
        </v-row>
        <div v-if="!createPage">
        <v-row>
            <v-col>
                <v-card class="elevation-2">
                    <v-toolbar
                            v-if="editPage"
                            color="primary"
                            dark
                            flat
                    >
                        <v-toolbar-title>Preview</v-toolbar-title>
                    </v-toolbar>
                    <v-card-actions>
                        <v-btn v-for="(item, index) in markingtext"  outlined tile rounded class="my-1" @click.local="deleteMarking(index)">{{item}}</v-btn>
                    </v-card-actions>
                    <v-card-text v-on:dblclick="showEditDialog">
                        <vue-markdown :source="currentPage"/>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
            <v-row v-if="editPage" align-content="center">
                <v-col align-self="center">
                    <v-btn class="my-1" @click.local="edit" color="primary">Save</v-btn>
                </v-col>
            </v-row>
        </div>
        <div v-if="!createPage&&!editPage">
        <v-row no-gutters>
            <v-expand-transition>
            <v-col>
                    <v-btn v-for="(item, index) in pages"  outlined tile rounded class="my-1" :color="$data.colors[index]" @click.local="viewPage(index)">{{index + 1}}</v-btn>
                <v-btn v-if="showCreateButton" class="ma-2" color="primary" @click.local="showCreationDialog">+</v-btn>
                <v-btn v-if="showCreateButton" class="ma-2" color="primary" @click.local="mark">Mark</v-btn>
            </v-col>
            </v-expand-transition>
        </v-row>
        </div>
        <v-btn v-if="!createPage&&!editPage" class="ma-2" color="primary" @click.local="deletePage">Delete</v-btn>
        <v-alert
                v-model="showError"
                type="error"
        >
            {{this.$data.errorMessage}}
        </v-alert>

        <v-alert
                v-model="showError"
                type="warning"
        >
            Are you sure you're using ASP.NET Core endpoint? (default at <a href="http://localhost:5000/fetch-data">http://localhost:5000</a>)<br>
            API call would fail with status code 404 when calling from Vue app (default at <a href="http://localhost:8080/fetch-data">http://localhost:8080</a>) without devServer proxy settings in vue.config.js file.
        </v-alert>

    </v-container>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import { Page } from '@/models/Page';
    import axios from 'axios';
    import {User} from "@/models/User";
    import router from "@/router";
    import VueMarkDown from "vue-markdown";
    import {PageRequest} from "@/models/PageRequest";
    import {Diary} from "@/models/Diary";
    import {Marking} from "@/models/Marking";
    
    
    @Component({components: { 'vue-markdown':VueMarkDown ,FetchDiaryView}})
    export default class FetchDiaryView extends Vue {
        private loading: boolean = true;
        private showError: boolean = false;
        private errorMessage: string = 'Error while loading diaries.';
        
        data() {
            return {
                pages: [],
                markings: [],
                markingtext: [],
                selected: [],
                colors:[],
                createPage: false,
                editPage: false,
                showCreateButton: true,
                showAlert: false,
                showError: false,
                selection: "",
                errorMessage: 'Error while loading diaries.',
                name: "",
                id : this.$route.params.id,
                lastNumber: -1,
                currentPageNum: -1,
                currentPageId: "",
                currentPage: "# There are no pages, yet. Try to create one."
            };
        }


        private created() {
            let authorized:string = localStorage.getItem('loggedIn') || "";
            if("true" != authorized){
                router.push('/login');
                return;
            }
            this.fetchPages();
            console.log(this.$route.params.id);
            var here = this;
            document.onmouseup = document.onkeyup = document.onselectionchange = function(){here.selectit();};
        }

        private showCreationDialog(){
            this.$data.createPage = !this.$data.createPage;
            this.$data.showCreateButton = !this.$data.showCreateButton;
        }

        private showEditDialog(){
            this.$data.editPage = !this.$data.editPage;
            this.$data.showCreateButton = !this.$data.showCreateButton;
        }

        private async fetchPages() {
            axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
            let colors : string[] = [];
            await axios
                .get<Page[]>(('api/Pages/GetPages/' + this.$data.id))
                .then((response) => {
                    this.$data.pages = response.data.sort((a, b) => (a.number > b.number) ? 1 : ((b.number > a.number) ? -1 : 0));
                    this.$data.lastNumber = this.$data.pages.length -1;
                    if (response.data.length > 0) {
                        response.data.forEach( function (item:Page) {
                            colors.push("primary");
                        });
                    }
                })
                .catch((e) => {
                    this.$data.showError = true;
                    console.log(e.response);
                    this.$data.errorMessage = `Error while loading diaries: ${e.message} + ${e.response.data}.`;
                    console.log(this.$data.errorMessage);
                })
                .finally(() => (this.loading = false));
            this.$data.colors = colors;
            if (this.$data.pages.length > 0) {
                this.viewPage(0);
            }
        }

        private async create(){
            axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
            let page :PageRequest = new PageRequest(this.$data.lastNumber + 1,this.$data.name);
            await axios
                .post<Page>('api/Pages/' + this.$data.id,
                    page,
                    {headers: {"Content-Type": "application/json"}})
                .catch((e) => {
                    this.$data.showError = true;
                    this.$data.errorMessage = `Error while creating diary ${this.$data.name}: ${e.message + e.response.data[0]}.`;
                    console.log(e.response.data);
                })
                .finally(() => (this.loading = false));
            this.fetchPages();
            this.showCreationDialog();
            this.diaryEdited();
        }

        private async edit(){
            axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
            let page :Page = new Page(this.$data.pages[this.$data.currentPageNum].id,this.$data.currentPageNum + 1,this.$data.currentPage);
            await axios
                .put<Page>('api/Pages/' + this.$data.id,
                    page,
                    {headers: {"Content-Type": "application/json"}})
                .catch((e) => {
                    this.$data.showError = true;
                    this.$data.errorMessage = `Error while editing diary page ${this.$data.name}: ${e.message + e.response.data[0]}.`;
                    console.log(e.response);
                })
                .finally(() => (this.loading = false));
            this.fetchPages();
            //this.showCreationDialog();
            this.showEditDialog();
            this.diaryEdited();
        }
        
        private async diaryEdited(){
            let diary:Diary = new Diary("", "", new Date(), new Date());
            axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
            await axios
                .get<Diary>('api/Diaries/' + this.$data.id)
                .then((response) => {
                    diary = response.data;
                    console.log(response.data);
                })
                .catch((e) => {
                    this.$data.showError = true;
                    this.$data.errorMessage = `Error while getting diary ${this.$data.name}: ${e.message + e.response.data[0]}.`;
                    console.log(e.response.data);
                });
            console.log(diary);
            diary.edited = new Date();
            await axios
                .put('api/Diaries/' + this.$data.id, diary)
                .catch((e) => {
                    this.$data.showError = true;
                    this.$data.errorMessage = `Error while updating diary ${this.$data.name}: ${e.message + e.response.data[0]}.`;
                    console.log(e.response.data);
                });
            
        }

        private async deleteDiaries() {
            let diaries = this.$data.diaries;
            this.$data.msg = this.$data.selected[0].name;
            await this.$data.selected.forEach( function (item:Page) {
                axios
                    .delete<Page[]>('api/Pages/' + item.id)
                    .then((response) => {
                    })
            });
            this.fetchPages();
        }
        
        private async deletePage(){
            console.log(this.$data.currentPageNum);
            let id = this.$data.pages[this.$data.currentPageNum].id;             
            axios.delete('api/Pages/' + id).catch((e) => {
                this.$data.showError = true;
                this.$data.errorMessage = `Error while deleting ${id}: ${e.message + e.response.data}.`;
                console.log(e.response);
            });
            this.fetchPages();
            this.diaryEdited();
        }


        private fetchPage(id:any){
            router.push("/fetch-page/" + id);
        }
        
        private viewPage(number:any){
            if(this.$data.currentPageNum >=0)
                this.$data.colors[this.$data.currentPageNum] = "primary";
            this.$data.currentPageNum = number;
            this.$data.currentPage = this.$data.pages[number].text;
            this.$data.currentPageId = this.$data.pages[number].id;
            this.$data.colors[number] = "red";
            this.fetchMarkings();
        }

        private selectit() {
            let text = this.getSelectionText();
            if(text != "")
                this.$data.selected = text;
        }

        private getSelectionText() {
            var text = "";
            var anydocument : any = document;
            var activeEl = document.activeElement;
            var activeElTagName = activeEl ? activeEl.tagName.toLowerCase() : null;
            if(activeElTagName != "button") {
                if (window.getSelection) {
                    text = (window.getSelection() || "").toString();
                } else if (anydocument.selection && anydocument.selection.type != "Control") {
                    text = anydocument.selection.createRange().text;
                }
            }
            //console.log(text);
            return text;
        }
        
        private async mark(){
            console.log(this.$data.selected);
            var text = this.$data.selected;
            var start = this.$data.currentPage.indexOf(text);
            var end = start + text.length;
            axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
            let marking : Marking = new Marking(this.$data.currentPageId,start,end, "");
            await axios
                .post('api/Markings/',
                    marking,
                    {headers: {"Content-Type": "application/json"}})
                .catch((e) => {
                    this.$data.showError = true;
                    this.$data.errorMessage = `Error while creating marking ${this.$data.name}: ${e.message + e.response.data[0]}.`;
                    console.log(e.response.data);
                })
                .finally(() => (this.loading = false));
            //this.fetchPages();
            //this.showCreationDialog();
            this.diaryEdited();
            this.viewPage(this.$data.currentPageNum);
        }
        
        private async fetchMarkings(){
            axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
            await axios
                .get<Marking[]>('api/Markings/GetMarkings/' +this.$data.currentPageId)
                .then((response) => {
                    this.$data.markings = response.data;
                    console.log(response.data);
                    console.log("hello");
                })
                .catch((e) => {
                    this.$data.showError = true;
                    this.$data.errorMessage = `Error while getting markings ${this.$data.name}: ${e.message + e.response.data[0]}.`;
                    console.log(e.response.data);
                    console.log("bye");
                })
                .finally(() => (this.loading = false));
            this.$data.markingtext = [];
            var markingtext: string[] = [];
            var self = this;
            this.$data.markings.forEach( function (item:Marking) {
                markingtext.push(self.$data.currentPage.substring(item.start, item.end));
            });
            this.$data.markingtext = markingtext;
            //this.showCreationDialog();
        }

        private async deleteMarking(index:number){
            axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
            await axios
                .delete('api/Markings/' + this.$data.markings[index].id)
                .catch((e) => {
                    this.$data.showError = true;
                    this.$data.errorMessage = `Error while deleting marking ${index}: ${e.message + e.response.data}.`;
                    console.log(e.response.data);
                    console.log("this.$data.markings[index].id)");
                });
            this.viewPage(this.$data.currentPageNum);
            //this.showCreationDialog();
        }
    }
          


    
</script>
